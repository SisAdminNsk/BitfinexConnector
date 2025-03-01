using StockExchangeCore.Abstract;
using static BitfinexUI.ViewModels.WsViewModel;
using System.Collections.ObjectModel;

namespace BitfinexUI.ViewModels
{
    public class WsCandlesViewModel : PageViewModel
    {
        private CurrencyPairsPannelViewModel _currencyPairsPannel = new();
        public ObservableCollection<CurrencyPair> Currencies { get => _currencyPairsPannel.Currencies; }

        private readonly IStockExchangeWsConnector _stockExchange;

        public WsCandlesViewModel(string header, IStockExchangeWsConnector stockExchange) : base(header) 
        {
            _stockExchange = stockExchange;    
        }
    }
}
