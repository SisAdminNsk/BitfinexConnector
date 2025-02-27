using ReactiveUI;
using System.Collections.ObjectModel;

namespace BitfinexUI.ViewModels
{
    public class RestViewModel : ViewModelBase
    {
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

            Tabs.Add(new TradesViewModel("Trades"));
        }
    }
}
