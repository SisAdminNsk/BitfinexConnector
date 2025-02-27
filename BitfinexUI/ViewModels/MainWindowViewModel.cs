using BitfinexConnector;
using StockExchangeCore.Abstract;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BitfinexUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        private readonly IStockExchangeRestConnector _stockExchangeRestConnector;

        public MainWindowViewModel()
        {
            var uri = "https://api-pub.bitfinex.com/v2/";

            _stockExchangeRestConnector = new BitfinexRestConnector(uri);

            LoadTradesCommand = new RelayCommand(async () => await LoadTradesAsync());
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