using Newtonsoft.Json.Linq;
using StockExchangeCore;

namespace BitfinexConnector
{
    internal class BitfinexCandleResponseConverter : ICandleResponseConverter
    {
        public IEnumerable<Candle> ConvertAll(string candleResponse, string? pair = "")
        {
            var result = new List<Candle>();

            var candles = JArray.Parse(candleResponse);

            foreach(var candle in candles)
            {
                var convertedCandle = new Candle
                {
                    Pair = pair,
                    OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(candle[0].Value<long>()),
                    OpenPrice = candle[1].Value<decimal>(),
                    ClosePrice = candle[2].Value<decimal>(),
                    HighPrice = candle[3].Value<decimal>(),
                    LowPrice = candle[4].Value<decimal>(),
                    TotalVolume = candle[5].Value<decimal>(),
                };

                convertedCandle.TotalPrice = convertedCandle.ClosePrice * convertedCandle.TotalVolume;

                result.Add(convertedCandle);
            }

            return result;
        }

        public Candle ConvertCandle(string candleResponse, string? pair = "")
        {
            var candleData = JArray.Parse(candleResponse);

            var candle = new Candle
            {
                Pair = pair,
                OpenTime = DateTimeOffset.FromUnixTimeMilliseconds(candleData[0].Value<long>()),
                OpenPrice = candleData[1].Value<decimal>(),
                ClosePrice = candleData[2].Value<decimal>(),
                HighPrice = candleData[3].Value<decimal>(),
                LowPrice = candleData[4].Value<decimal>(),
                TotalVolume = candleData[5].Value<decimal>()
            };

            candle.TotalPrice = candle.ClosePrice * candle.TotalVolume;

            return candle;
        }
    }
}
