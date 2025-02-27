namespace StockExchangeCore
{
    public interface ITradeResponseConverter
    {
        IEnumerable<Trade> ConvertAll(string tradeResponse, string? pair = "");
        Trade ConvertTrade (string tradeResponse, string? pair="");
    }
}
