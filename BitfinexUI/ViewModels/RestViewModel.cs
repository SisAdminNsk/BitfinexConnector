using ReactiveUI;
using System.Collections.ObjectModel;
using BitfinexConnector;
using StockExchangeCore.Abstract;

namespace BitfinexUI.ViewModels
{
    public class RestViewModel : PageViewModel
    {
        private string _selectedCurrencyPair;
        public string SelectedCurrencyPair
        {
            get => _selectedCurrencyPair;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedCurrencyPair, value);
            }
        }

        public ObservableCollection<string> CurrencyPairs { get; } = new();

        private ObservableCollection<ViewModelBase> _tabs =
            new ObservableCollection<ViewModelBase>();

        public ObservableCollection<ViewModelBase> Tabs
        {
            get => _tabs;
            private set => this.RaiseAndSetIfChanged(ref _tabs, value);
        }

        public RestViewModel(string header, IStockExchangeRestConnector stockExchange) : base(header)
        {
            Tabs.Add(new TradesViewModel("Trades", this, stockExchange));
            Tabs.Add(new CandlesViewModel("Candles", this, stockExchange));
            Tabs.Add(new TickerViewModel("Ticker", this, stockExchange));

            var pairs = BitfinexUtils.GetAvaliableCurrencyPairs();

            foreach (var pair in pairs)
            {
                CurrencyPairs.Add(pair);
            }

            SelectedCurrencyPair = CurrencyPairs[0];
        }
    }
}
