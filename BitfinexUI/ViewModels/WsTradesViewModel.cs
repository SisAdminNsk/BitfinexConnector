using StockExchangeCore.Abstract;

namespace BitfinexUI.ViewModels
{
    public class WsTradesViewModel : PageViewModel
    {
        public WsTradesViewModel(string header, WsViewModel parent, IStockExchangeWsConnector stockExchange) : base(header) 
        {
            
        }
    }
}
