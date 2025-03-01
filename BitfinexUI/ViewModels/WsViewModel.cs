using BitfinexConnector;
using ReactiveUI;
using StockExchangeCore.Abstract;
using System.Collections.ObjectModel;

namespace BitfinexUI.ViewModels
{
    public partial class WsViewModel : PageViewModel
    {
        public ObservableCollection<CurrencyPair> Currencies { get; set; } = new();

        private ObservableCollection<ViewModelBase> _tabs =
          new ObservableCollection<ViewModelBase>();

        public ObservableCollection<ViewModelBase> Tabs
        {
            get => _tabs;
            private set => this.RaiseAndSetIfChanged(ref _tabs, value);
        }

        public WsViewModel(string header) : base(header) 
        {
            IStockExchangeWsConnector connector = new BitfinexWsConnector("wss://api-pub.bitfinex.com/ws/2");

            Tabs.Add(new WsTradesViewModel("Trades", this, connector));
            Tabs.Add(new WsCandlesViewModel("Candles", this, connector));

            var pairs = BitfinexUtils.GetAvaliableCurrencyPairs();

            foreach(var pair in pairs)
            {
                Currencies.Add(new CurrencyPair() {Name = pair, SubscribeCommand = ReactiveCommand.Create<object>(OnSubscribe)});
            }

            //Currencies = new ObservableCollection<CurrencyPair>
            //{
            //    new CurrencyPair { Name = "USD", IsSubscribed = false, SubscribeCommand = ReactiveCommand.Create<object>(OnSubscribe) },
            //    new CurrencyPair { Name = "EUR", IsSubscribed = true, SubscribeCommand = ReactiveCommand.Create<object>(OnSubscribe) },
            //    new CurrencyPair { Name = "GBP", IsSubscribed = false, SubscribeCommand = ReactiveCommand.Create<object>(OnSubscribe) }
            //};
        }

        private void OnSubscribe(object parameter)
        {
            if (parameter is CurrencyPair currency)
            {
                currency.IsSubscribed = !currency.IsSubscribed;
            }
        }
    }
}
