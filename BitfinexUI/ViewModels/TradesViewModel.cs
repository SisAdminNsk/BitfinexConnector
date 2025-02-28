using ReactiveUI;
using StockExchangeCore.StockModels;
using System.Collections.ObjectModel;
using StockExchangeCore.Abstract;
using BitfinexConnector;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BitfinexUI.ViewModels
{
    public class TradesViewModel : PageViewModel
    {
        public ICommand LoadTradesCommand { get; }

        private readonly IStockExchangeRestConnector _stockExchange;
        private readonly RestViewModel _restViewModel;

        private int _tradesCount = 20;
        public int TradesCount { get => _tradesCount; private set => this.RaiseAndSetIfChanged(ref _tradesCount, value); }

        private ObservableCollection<Trade> _trades = new();
        public ObservableCollection<Trade> Trades
        {
            get => _trades;
            set => this.RaiseAndSetIfChanged(ref _trades, value);
        }

        public TradesViewModel(string header, RestViewModel restViewModel, IStockExchangeRestConnector stockExchange) : base(header)
        {
            _restViewModel = restViewModel;

            _stockExchange = stockExchange;

            LoadTradesCommand = new RelayCommand(async () => await LoadTradesAsync());
        }

        public void IncreaseTradesCount()
        {
            if(TradesCount < 100)
            {
                TradesCount++;
            }
        }

        public void DecreaseTradesCount()
        {
            if(TradesCount > 0)
            {
                TradesCount--;
            }
        }

        public async Task LoadTradesAsync()
        {
            var selectedPair = _restViewModel.SelectedCurrencyPair; 

            var trades = await _stockExchange.GetNewTradesAsync(selectedPair, TradesCount);

            Trades.Clear();
    
            foreach (var trade in trades)
            {
                Trades.Add(trade);
            }
        }
    }
}
