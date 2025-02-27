namespace StockExchangeCore
{
    public interface ICandleResponseConverter
    {
        IEnumerable<Candle> ConvertAll(string candleResponse, string? pair = "");

        Candle ConvertCandle(string candleResponse, string? pair = "");
    }
}
