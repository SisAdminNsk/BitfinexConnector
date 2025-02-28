using ReactiveUI;

namespace BitfinexUI.ViewModels
{
    public class PageViewModel : ViewModelBase
    {
        private string _header = "";
        public string Header { get => _header; private set => this.RaiseAndSetIfChanged(ref _header, value); }

        public PageViewModel(string header)
        {
            Header = header;
        }
    }
}
