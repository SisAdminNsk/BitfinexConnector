using StockExchangeCore.Abstract;
using StockExchangeCore.StockModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BitfinexUI.ViewModels
{
    internal class TickerViewModel : PageViewModel
    {
        private readonly IStockExchangeRestConnector _stockExchange;
        public ICommand LoadTickerCommand { get; }

        private readonly RestViewModel _restViewModel;

        public ObservableCollection<Ticker> Tickers { get; private set; } = new();

        public TickerViewModel(string header, RestViewModel parent, IStockExchangeRestConnector restConnector) : base(header)
        {
            _stockExchange = restConnector;
            _restViewModel = parent;

            LoadTickerCommand = new RelayCommand(async () => await LoadTickerAsync());
        }

        public async Task LoadTickerAsync()
        {
            var selectedPair = _restViewModel.SelectedCurrencyPair;

            var ticker = await _stockExchange.GetTickerAsync(selectedPair);

            ticker.Pair = selectedPair;

            Tickers.Clear();
            Tickers.Add(ticker);
        }
    }
}
