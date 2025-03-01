using ReactiveUI;
using StockExchangeCore.Abstract;
using System.Collections.ObjectModel;

namespace BitfinexUI.ViewModels
{
    public partial class WsViewModel : PageViewModel
    {
        private ObservableCollection<ViewModelBase> _tabs =
          new ObservableCollection<ViewModelBase>();

        public ObservableCollection<ViewModelBase> Tabs
        {
            get => _tabs;
            private set => this.RaiseAndSetIfChanged(ref _tabs, value);
        }

        public WsViewModel(string header, IStockExchangeWsConnector stockExchange) : base(header) 
        {
            Tabs.Add(new WsTradesViewModel("Trades", stockExchange));
            Tabs.Add(new WsCandlesViewModel("Candles", stockExchange));
        }
    }
}
