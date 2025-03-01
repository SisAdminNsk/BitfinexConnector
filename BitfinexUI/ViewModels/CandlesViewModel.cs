using ReactiveUI;
using StockExchangeCore.Abstract;
using StockExchangeCore.StockModels;
using System.Collections.ObjectModel;
using System.Linq;
using BitfinexConnector;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BitfinexUI.ViewModels
{
    public class CandlesViewModel : PageViewModel
    {
        private readonly IStockExchangeRestConnector _stockExchange;
        public ICommand LoadCandlesCommand { get; }

        private readonly RestViewModel _restViewModel;

        private int _periodInSec = 60;
        public int PeriodInSec { get => _periodInSec; private set => this.RaiseAndSetIfChanged(ref _periodInSec, value); }


        private int _candlesCount = 20;
        public int CandlesCount { get => _candlesCount; private set => this.RaiseAndSetIfChanged(ref _candlesCount, value); }
        public ObservableCollection<Candle> Candles { get; set; } = new();

        private string _selectedPeriod;
        public string SelectedPeriod
        {
            get => _selectedPeriod;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedPeriod, value);
            }
        }

        public ObservableCollection<string> TimePeriods { get; } =
            new ObservableCollection<string>(BitfinexUtils.GetAvailablePeriodInSec().Keys.ToList());


        public CandlesViewModel(string header, RestViewModel parent, IStockExchangeRestConnector stockExchange) : base(header)
        {
            _restViewModel = parent;

            _stockExchange = stockExchange;

            LoadCandlesCommand = ReactiveCommand.Create(async () => await LoadCandlesAsync());

            SelectedPeriod = TimePeriods[0];
        }

        public void IncreaseCandlesCount()
        {
            if (CandlesCount < 100)
            {
                CandlesCount++;
            }
        }

        public void DecreaseCandlesCount()
        {
            if (CandlesCount > 0)
            {
                CandlesCount--;
            }
        }

        public async Task LoadCandlesAsync()
        {
            var selectedPair = _restViewModel.SelectedCurrencyPair;

            PeriodInSec = BitfinexUtils.ConvertPeriodToTimeFrame(SelectedPeriod);

            var candles = await _stockExchange.GetCandleSeriesAsync(selectedPair, PeriodInSec, null, null, count: CandlesCount);

            Candles.Clear();

            foreach (var candle in candles)
            {
                Candles.Add(candle);
            }
        }
    }
}
