using StockExchangeCore.StockModels;

namespace StockExchangeCore.Abstract
{
    public interface ITradeResponseConverter
    {
        IEnumerable<Trade> ConvertAll(string tradeResponse, string? pair = "");
        Trade ConvertTrade(string tradeResponse, string? pair = "");
    }
}
