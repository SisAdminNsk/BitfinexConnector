using BitfinexConnector;
using ReactiveUI;
using StockExchangeCore.Abstract;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private readonly IStockExchangeRestConnector _stockExchangeRestConnector;

        public MainWindowViewModel()
        {
            var uri = "https://api-pub.bitfinex.com/v2/";

            _stockExchangeRestConnector = new BitfinexRestConnector(uri);

            LoadTradesCommand = new RelayCommand(async () => await LoadTradesAsync());

            Tabs.Add(new RestViewModel("Rest"));
            Tabs.Add(new RestViewModel("Websocket"));
        }

        public ICommand LoadTradesCommand { get; }

        private async Task LoadTradesAsync()
        {
            var trades = await _stockExchangeRestConnector.GetNewTradesAsync("BTCUSD", 100);

            foreach (var trade in trades)
            {
                Console.WriteLine($"Trade: {trade.Pair}, Price: {trade.Price}, Amount: {trade.Amount}, Side: {trade.Side}");
            }
        }
    }
}