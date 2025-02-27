namespace StockExchangeCore
{
    public interface ITickerResponseConverter
    {
        Ticker Convert(string tickerResponse);
    }
}
