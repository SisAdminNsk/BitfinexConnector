using StockExchangeCore.StockModels;

namespace StockExchangeCore.Abstract
{
    public interface ICandleResponseConverter
    {
        IEnumerable<Candle> ConvertAll(string candleResponse, string? pair = "");

        Candle ConvertCandle(string candleResponse, string? pair = "");
    }
}
