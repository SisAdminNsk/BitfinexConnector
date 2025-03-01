using static BitfinexUI.ViewModels.WsViewModel;
using System.Collections.ObjectModel;
using BitfinexConnector;
using ReactiveUI;
using System;

namespace BitfinexUI.ViewModels
{
    public class CurrencyPairsPannelViewModel
    {
        public event Action<CurrencyPair>? CurrencyPairStateChanged;

        public ObservableCollection<CurrencyPair> Currencies { get; set; } = new();

        public CurrencyPairsPannelViewModel()
        {
            var pairs = BitfinexUtils.GetAvaliableCurrencyPairs();

            foreach (var pair in pairs)
            {
                Currencies.Add(new CurrencyPair() { Name = pair, SubscribeCommand = ReactiveCommand.Create<object>(OnSubscribe) });
            }
        }

        private void OnSubscribe(object parameter)
        {
            if (parameter is CurrencyPair currency)
            {
                currency.IsSubscribed = !currency.IsSubscribed;

                CurrencyPairStateChanged?.Invoke(currency);
            }
        }
    }
}
