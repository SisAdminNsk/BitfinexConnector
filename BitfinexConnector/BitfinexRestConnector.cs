using RestSharp;
using StockExchangeCore.Abstract;
using StockExchangeCore.StockModels;

namespace BitfinexConnector
{
    public class BitfinexRestConnector : IStockExchangeRestConnector
    {
        private readonly RestClient _client;
        public BitfinexRestConnector(string baseBinfinexApiUrl)
        {
            var options = new RestClientOptions(baseBinfinexApiUrl);
            _client = new RestClient(options);
        }

        public async Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            var content = await ExecuteGettingCandlesAsync(pair, periodInSec, from, to, count);

            ICandleResponseConverter candleResponseConverter = new BitfinexCandleResponseConverter();

            var candles = candleResponseConverter.ConvertAll(content, pair);

            return candles;
        }

        private async Task<string> ExecuteGettingCandlesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            var url = BuildCandlesRequestUrl(pair, periodInSec, from, to, count);

            var request = new RestRequest(url);
            request.AddHeader("accept", "application/json");

            var response = await _client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new HttpRequestException($"Ошибка при выполнении http запроса: {response.StatusCode}," +
                    $" сообщение: {response.ErrorMessage}");
            }

            return response.Content;
        }

        private string BuildCandlesRequestUrl(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0)
        {
            var timeFrame = BitfinexUtils.ConvertPeriodToTimeFrame(periodInSec);

            var url = $"candles/trade:{timeFrame}:t{pair}/hist?limit={count}";

            if (from.HasValue)
            {
                url += $"&start={from.Value.ToUnixTimeMilliseconds()}";
            }
            if (to.HasValue)
            {
                url += $"&end={to.Value.ToUnixTimeMilliseconds()}";
            }

            return url;
        }

        public async Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount)
        {
            var content = await ExecuteGettingTradesAsync(pair, maxCount);

            ITradeResponseConverter tradeResponseConverter = new BitfinexTradeResponseConverter();

            var trades = tradeResponseConverter.ConvertAll(content, pair);

            return trades;
        }

        private async Task<string> ExecuteGettingTradesAsync(string pair, int maxCount)
        {
            var request = new RestRequest($"/trades/t{pair}/hist?limit={maxCount}");
            request.AddHeader("accept", "application/json");

            var response = await _client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new HttpRequestException($"Ошибка при выполнении http запроса: {response.StatusCode}," +
                    $" сообщение: {response.ErrorMessage}");
            }

            return response.Content;
        }

        public async Task<Ticker> GetTickerAsync(string pair)
        {
            var content = await ExecuteGettingTickerAsync(pair);

            ITickerResponseConverter tickerResponseConverter = new BitfinexTickerResponseConverter();

            var ticker = tickerResponseConverter.Convert(content);

            return ticker;
        }

        private async Task<string> ExecuteGettingTickerAsync(string pair)
        {
            var request = new RestRequest($"/ticker/t{pair}");
            request.AddHeader("accept", "application/json");

            var response = await _client.GetAsync(request);

            if (!response.IsSuccessful)
            {
                throw new HttpRequestException($"Ошибка при выполнении http запроса: {response.StatusCode}," +
                   $" сообщение: {response.ErrorMessage}");
            }

            return response.Content;
        }
    }
}