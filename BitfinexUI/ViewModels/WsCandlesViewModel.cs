using StockExchangeCore.Abstract;

namespace BitfinexUI.ViewModels
{
    public class WsCandlesViewModel : PageViewModel
    {
        public WsCandlesViewModel(string header, WsViewModel parent, IStockExchangeWsConnector stockExchange) : base(header) 
        {
            
        }
    }
}
