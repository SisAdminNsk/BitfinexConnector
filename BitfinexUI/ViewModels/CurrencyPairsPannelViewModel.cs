using static BitfinexUI.ViewModels.WsViewModel;
using System.Collections.ObjectModel;
using BitfinexConnector;
using ReactiveUI;
using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;

namespace BitfinexUI.ViewModels
{
    public class CurrencyPairsPannelViewModel
    {
        private readonly BehaviorSubject<bool> _isBlocked = new BehaviorSubject<bool>(false);

        public event Action<CurrencyPair>? CurrencyPairStateChanged;

        public ObservableCollection<CurrencyPair> Currencies { get; set; } = new();

        public CurrencyPairsPannelViewModel()
        {
            var pairs = BitfinexUtils.GetAvaliableCurrencyPairs();

            foreach (var pair in pairs)
            {
                var canSub = CanSubscribe();

                Currencies.Add(new CurrencyPair()
                {
                    Name = pair,
                    SubscribeCommand = ReactiveCommand.Create<object>(OnSubscribe, canSub)
                });
            }
        }

        public void Block()
        {
            _isBlocked.OnNext(true);
        }

        public void Unblock()
        {
            _isBlocked.OnNext(false);
        }


        private void OnSubscribe(object parameter)
        {
            if (parameter is CurrencyPair currency)
            {
                currency.IsSubscribed = !currency.IsSubscribed;

                CurrencyPairStateChanged?.Invoke(currency);
            }
        }

        private IObservable<bool> CanSubscribe()
        {
            return _isBlocked.Select(isBlocked => !isBlocked); 
        }
    }
}
