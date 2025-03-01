using Newtonsoft.Json.Linq;
using StockExchangeCore;
using StockExchangeCore.Abstract;
using StockExchangeCore.StockModels;

namespace BitfinexConnector
{
    public class BitfinexWsConnector : IStockExchangeWsConnector
    {
        private bool _connectedToTradesChannel = false;
        private bool _connectedToCandlesChannel = false;

        private readonly Dictionary<int, string> _tradeChannelIdToPair = new();
        private readonly Dictionary<int, string> _candleChannelIdToPair = new();

        private readonly WebSocketClient _wsTradeClient;
        private readonly WebSocketClient _wsCandleClient;
        
        public event Action<Trade> NewBuyTrade;
        public event Action<Trade> NewSellTrade;
        public event Action<Candle> CandleSeriesProcessing;

        public event Action<ErrorArgs> Error;

        public BitfinexWsConnector(string baseBitfinexWsApiUrl)
        {
            _wsTradeClient = new WebSocketClient(baseBitfinexWsApiUrl);
            _wsCandleClient = new WebSocketClient(baseBitfinexWsApiUrl);

            _wsTradeClient.OnMessageReceived += ProcessTradesWsMessage;
            _wsCandleClient.OnMessageReceived += ProcessCandlesWsMessage;
        }

        private async Task ConnectToTradesAsync()
        {
            await _wsTradeClient.ConnectAsync();
            _connectedToTradesChannel = true;
        }
        private async Task ConnectToCandlesAsync()
        {
            await _wsCandleClient.ConnectAsync();
            _connectedToCandlesChannel = true;
        }

        public async Task SubscribeTradesAsync(string pair, int maxCount = 100)
        {
            if (!_connectedToTradesChannel)
            {
                await ConnectToTradesAsync();
            }

            var msg = $"{{\"event\":\"subscribe\", \"channel\":\"trades\", \"symbol\":\"t{pair}\"}}";

            await _wsTradeClient.SendMessageAsync(msg);
        }

        public async Task UnsubscribeTradesAsync(string pair)
        {
            var chanId = _tradeChannelIdToPair.FirstOrDefault(x => x.Value == pair).Key;

            var msg = $"{{\"event\":\"unsubscribe\", \"chanId\":\"{chanId}\"}}";

            await _wsTradeClient.SendMessageAsync(msg);
        }

        public async Task SubscribeCandlesAsync(string pair, int periodInSec, DateTimeOffset? from = null, DateTimeOffset? to = null, long? count = 0)
        {
            if (!_connectedToCandlesChannel)
            {
                await ConnectToCandlesAsync();
            }

            var timeFrame = BitfinexUtils.ConvertPeriodToTimeFrame(periodInSec);

            string key = $"trade:{timeFrame}:t{pair}";

            var msg = $"{{\"event\":\"subscribe\", \"channel\":\"candles\", \"key\":\"{key}\"}}";

            await _wsCandleClient.SendMessageAsync(msg);
        }

        public async Task UnsubscribeCandlesAsync(string pair)
        {
            var chanId = _candleChannelIdToPair.FirstOrDefault(x => x.Value == pair).Key;

            var msg = $"{{\"event\":\"unsubscribe\", \"chanId\":\"{chanId}\"}}";

            await _wsCandleClient.SendMessageAsync(msg);
        }
        private void ProcessTradesWsMessage(string message)
        {
            try
            {
                var token = JToken.Parse(message);

                ProcessIfEventMessage(token, ProcessWhenTradesSubscribedEvent);

                ProcessIfTradeUpdateMessage(token);
            }
            catch(Exception ex)
            {
                Error?.Invoke(new ErrorArgs()
                {
                    ErrorMessage = ex.Message,
                    ErorrDescriprion = $"Возникла ошибка при обработке запроса: {BitfinexUtils.GetCurrentMethodFullName()}"
                });
            }
        }

        private void ProcessIfEventMessage(JToken token, Action<JObject> onSubscribeEventHandler)
        {
            if (token is JObject jObject)
            {
                if (jObject.ContainsKey("event"))
                {
                    ProcessEventMessage(jObject, onSubscribeEventHandler);
                }
            }
        }

        private void ProcessWhenTradesSubscribedEvent(JObject jObject)
        {
            var chanId = int.Parse(jObject["chanId"].ToString());
            var pair = jObject["pair"].ToString();

            _tradeChannelIdToPair.Add(chanId, pair);
        }

        private void ProcessEventMessage(JObject jObject, Action<JObject> onSubscribeEventHandler)
        {
            string eventMessage = jObject["event"].ToString();

            if (eventMessage == "subscribed")
            {
                onSubscribeEventHandler(jObject);
            }

            if (eventMessage == "unsubscribed")
            {
                var status = jObject["status"].ToString();

                if (status == "OK")
                {
                    var chanId = int.Parse(jObject["chanId"].ToString());
                    _tradeChannelIdToPair.Remove(chanId);
                }
            }

            if (eventMessage == "error")
            {
                Error?.Invoke(new ErrorArgs()
                {
                    ErrorMessage = jObject["msg"].ToString(),
                    ErorrDescriprion = $"Возникла ошибка при обработке запроса: {BitfinexUtils.GetCurrentMethodFullName()}"
                });
            }
        }
        private void ProcessIfTradeUpdateMessage(JToken token)
        {
            if (token is JArray jArray)
            {
                // Если второй элемент массива - это строка, это сообщение типа "update"
                if (jArray.Count > 1 && jArray[1].Type == JTokenType.String)
                {
                    ProcessTradeUpdateMessage(jArray);
                }
            }
        }

        private void ProcessTradeUpdateMessage(JArray data)
        {
            int chanId = int.Parse(data[0].ToString());
            string messageType = data[1].ToString();

            // Обрабатываем только сообщения типа "te" (trade execution) или "tu" (trade update)
            if (messageType == "te" || messageType == "tu")
            {
                var tradeData = data[2] as JArray;

                if (tradeData != null)
                {
                    ITradeResponseConverter tradeResponseConverter = new BitfinexTradeResponseConverter();

                    var trade = tradeResponseConverter.ConvertTrade(tradeData.ToString(), _tradeChannelIdToPair[chanId]);

                    if (trade.Side == "buy")
                    {
                        NewBuyTrade?.Invoke(trade);
                    }
                    else
                    {
                        NewSellTrade?.Invoke(trade);
                    }
                }
            }
        }
        private void ProcessCandlesWsMessage(string message)
        {
            try
            {
                var token = JToken.Parse(message);

                ProcessIfEventMessage(token, ProcessWhenCandlesSubsribedEvent);

                ProcessIfCandlesUpdateMessage(token);
            }
            catch (Exception ex)
            {
                Error?.Invoke(new ErrorArgs()
                {
                    ErrorMessage = ex.Message,
                    ErorrDescriprion = $"Возникла ошибка при обработке запроса: {BitfinexUtils.GetCurrentMethodFullName()}"
                });
            }
        }
        private void ProcessWhenCandlesSubsribedEvent(JObject jObject)
        {
            var key = jObject["key"].ToString();
            var chanId = int.Parse(jObject["chanId"].ToString());

            var parts = key.Split(':');

            var pair = parts[2].Substring(1);

            _candleChannelIdToPair.Add(chanId, pair);
        }

        private void ProcessIfCandlesUpdateMessage(JToken token)
        {
            if (token is JArray jArray)
            {
                // Если второй элемент массива - это массив, это сообщение типа "update"
                if (jArray.Count > 1 && jArray[1].Type == JTokenType.Array)
                {
                    ProcessCandlesUpdateMessage(jArray);
                }
            }
        }

        private void ProcessCandlesUpdateMessage(JArray data)
        {
            int chanId = data[0].Value<int>();
            var candleData = data[1] as JArray;

            if (candleData != null)
            {
                ICandleResponseConverter candleResponseConverter = new BitfinexCandleResponseConverter();

                var candle = candleResponseConverter.ConvertCandle(candleData.ToString(), _candleChannelIdToPair[chanId]);

                CandleSeriesProcessing?.Invoke(candle);
            }
        }
    }
}