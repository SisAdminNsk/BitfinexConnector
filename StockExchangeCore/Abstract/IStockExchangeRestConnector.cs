using StockExchangeCore.StockModels;

namespace StockExchangeCore.Abstract
{
    public interface IStockExchangeRestConnector
    {
        Task<IEnumerable<Trade>> GetNewTradesAsync(string pair, int maxCount);
        Task<IEnumerable<Candle>> GetCandleSeriesAsync(string pair, int periodInSec, DateTimeOffset? from, DateTimeOffset? to = null, long? count = 0);
        Task<Ticker> GetTickerAsync(string pair);
    }
}
