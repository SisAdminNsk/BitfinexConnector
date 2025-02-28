using ReactiveUI;
using System.Collections.ObjectModel;

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

        public MainWindowViewModel()
        {
            Tabs.Add(new RestViewModel("Rest"));
            Tabs.Add(new RestViewModel("Websocket"));
        }
    }
}