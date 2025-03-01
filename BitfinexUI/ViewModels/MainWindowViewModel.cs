using BitfinexConnector;
using ReactiveUI;
using StockExchangeCore.Abstract;
using System.Collections.ObjectModel;

namespace BitfinexUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private ObservableCollection<ViewModelBase> _tabs =
            new ObservableCollection<ViewModelBase>();

        public ObservableCollection<ViewModelBase> Tabs
        {
            get => _tabs;
            private set => this.RaiseAndSetIfChanged(ref _tabs, value);
        }

        public MainWindowViewModel()
        {
            IStockExchangeRestConnector restConnector = new BitfinexRestConnector("https://api-pub.bitfinex.com/v2/");
            IStockExchangeWsConnector wsConnector = new BitfinexWsConnector("wss://api-pub.bitfinex.com/ws/2");

            Tabs.Add(new RestViewModel("Rest", restConnector));
            Tabs.Add(new WsViewModel("Websocket", wsConnector));
            Tabs.Add(new PortfolioViewModel("Portfolio", restConnector));
        }
    }
}