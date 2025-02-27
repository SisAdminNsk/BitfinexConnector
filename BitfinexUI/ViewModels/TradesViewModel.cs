using ReactiveUI;
using StockExchangeCore.StockModels;
using System.Collections.ObjectModel;
using System;

namespace BitfinexUI.ViewModels
{
    public class TradesViewModel : ViewModelBase
    {
        private string _header = "";
        public string Header { get => _header; private set => this.RaiseAndSetIfChanged(ref _header, value); }

        private readonly RestViewModel _restViewModel;
        public TradesViewModel(string header, RestViewModel restViewModel)
        {
            Header = header;
            _restViewModel = restViewModel;

            Trades = new ObservableCollection<Trade>();
        }

        private ObservableCollection<Trade> _trades = new();
        public ObservableCollection<Trade> Trades
        {
            get => _trades;
            set => this.RaiseAndSetIfChanged(ref _trades, value);
        }

        public void LoadTrades()
        {
            var selectedPair = _restViewModel.SelectedCurrencyPair; // будет использоваться для запроса к коннектору биржи
           // var value = selectedPair.;

            // Пример: Загружаем тестовые данные
            var trades = new ObservableCollection<Trade>
            {
                new Trade
                {
                    Pair = "BTCUSD",
                    Price = 50000,
                    Amount = 0.1m,
                    Side = "buy",
                    Time = DateTimeOffset.Now,
                    Id = "12345"
                },
                new Trade
                {
                    Pair = "BTCUSD",
                    Price = 51000,
                    Amount = 0.2m,
                    Side = "sell",
                    Time = DateTimeOffset.Now,
                    Id = "67890"
                }
            };

            // Очищаем коллекцию и добавляем новые данные
            Trades.Clear();
            foreach (var trade in trades)
            {
                Trades.Add(trade);
            }
        }
    }
}
