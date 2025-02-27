using StockExchangeCore.StockModels;

namespace StockExchangeCore.Abstract
{
    public interface ITickerResponseConverter
    {
        Ticker Convert(string tickerResponse);
    }
}
