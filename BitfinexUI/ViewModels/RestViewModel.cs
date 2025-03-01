using ReactiveUI;
using System.Collections.ObjectModel;
using BitfinexConnector;

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

        public RestViewModel(string header) : base(header)
        {
            var connector = new BitfinexRestConnector("https://api-pub.bitfinex.com/v2/");

            Tabs.Add(new TradesViewModel("Trades", this, connector));
            Tabs.Add(new CandlesViewModel("Candles", this, connector));
            Tabs.Add(new TickerViewModel("Ticker", this, connector));

            var pairs = BitfinexUtils.GetAvaliableCurrencyPairs();

            foreach (var pair in pairs)
            {
                CurrencyPairs.Add(pair);
            }

            SelectedCurrencyPair = CurrencyPairs[0];
        }
    }
}
