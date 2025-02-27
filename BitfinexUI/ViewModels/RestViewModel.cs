using ReactiveUI;
using System;
using System.Collections.ObjectModel;

namespace BitfinexUI.ViewModels
{
    public class RestViewModel : ViewModelBase
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

        public ObservableCollection<string> CurrencyPairs { get; } = new ObservableCollection<string>
        {
            "BTCUSD",
            "XRPUSD",
            "XMRUSD",
            "DASHUSD"
        };

        private ObservableCollection<ViewModelBase> _tabs =
            new ObservableCollection<ViewModelBase>();

        public ObservableCollection<ViewModelBase> Tabs
        {
            get => _tabs;
            private set => this.RaiseAndSetIfChanged(ref _tabs, value);
        }

        public string Header { get; }

        public RestViewModel(string header)
        {
            Header = header;

            Tabs.Add(new TradesViewModel("Trades", this));

            SelectedCurrencyPair = CurrencyPairs[0];
        }
    }
}
