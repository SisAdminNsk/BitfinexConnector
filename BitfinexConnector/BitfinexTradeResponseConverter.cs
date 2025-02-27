using Newtonsoft.Json.Linq;
using StockExchangeCore.Abstract;
using StockExchangeCore.StockModels;
using System.Text.Json.Nodes;

namespace BitfinexConnector
{
    internal class BitfinexTradeResponseConverter : ITradeResponseConverter
    {
        public IEnumerable<Trade> ConvertAll(string tradeResponse, string pair)
        {
            var result = new List<Trade>();

            var trades = JArray.Parse(tradeResponse);

            foreach (var trade in trades)
            {
                result.Add(new Trade
                {
                    Pair = pair,
                    Id = trade[0].Value<string>(),
                    Amount = Math.Abs(trade[2].Value<decimal>()),
                    Price = trade[3].Value<decimal>(),
                    Side = trade[2].Value<decimal>() > 0 ? "buy" : "sell",
                    Time = DateTimeOffset.FromUnixTimeMilliseconds(trade[1].Value<long>())
                });
            }

            return result;
        }

        public Trade ConvertTrade(string tradeResponse, string? pair = "")
        {
            var tradeData = JArray.Parse(tradeResponse);

            var trade = new Trade
            {
                Id = tradeData[0].Value<string>(),
                Time = DateTimeOffset.FromUnixTimeMilliseconds(tradeData[1].Value<long>()),
                Amount = Math.Abs(tradeData[2].Value<decimal>()),
                Price = tradeData[3].Value<decimal>(),
                Side = tradeData[2].Value<decimal>() > 0 ? "buy" : "sell",
                Pair = pair
            };

            return trade;
        }
    }
}
