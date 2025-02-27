using Newtonsoft.Json;
using StockExchangeCore.Abstract;
using StockExchangeCore.StockModels;

namespace BitfinexConnector
{
    internal class BitfinexTickerResponseConverter : ITickerResponseConverter
    {
        public Ticker Convert(string tickerResponse)
        {
            var tickerArray = JsonConvert.DeserializeObject<double[]>(tickerResponse);

            return new Ticker
            {
                Bid = tickerArray[0],
                BidSize = tickerArray[1],
                Ask = tickerArray[2],
                AskSize = tickerArray[3],
                DailyChange = tickerArray[4],
                DailyChangeRelative = tickerArray[5],
                LastPrice = tickerArray[6],
                Volume = tickerArray[7],
                High = tickerArray[8],
                Low = tickerArray[9]
            };
        }
    }
}
