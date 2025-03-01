using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BitfinexUI.ViewModels
{
    public partial class WsViewModel
    {
        public class CurrencyPair : INotifyPropertyChanged
        {
            private bool _isSubscribed = false;
            public string Name { get; set; }

            public bool IsSubscribed
            {
                get => _isSubscribed;
                set
                {
                    _isSubscribed = value;
                    OnPropertyChanged();
                }
            }

            public ICommand SubscribeCommand { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
