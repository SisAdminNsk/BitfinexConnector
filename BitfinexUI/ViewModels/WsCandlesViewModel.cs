using StockExchangeCore.Abstract;
using static BitfinexUI.ViewModels.WsViewModel;
using System.Collections.ObjectModel;
using BitfinexUI.Common;
using StockExchangeCore.StockModels;
using System.Windows.Input;
using System.Threading.Tasks;
using ReactiveUI;

namespace BitfinexUI.ViewModels
{
    public class WsCandlesViewModel : PageViewModel
    {
        private CurrencyPairsPannelViewModel _currencyPairsPannel = new();
        public ObservableCollection<CurrencyPair> Currencies { get => _currencyPairsPannel.Currencies; }

        public ObservableStack<Candle> Candles { get; set; } = new();

        private readonly IStockExchangeWsConnector _stockExchange;

        public ICommand ClearCandlesCommand { get; }

        public WsCandlesViewModel(string header, IStockExchangeWsConnector stockExchange) : base(header) 
        {
            _stockExchange = stockExchange;

            _currencyPairsPannel.CurrencyPairStateChanged += async currency => await SubscribeForCurrencyStateChanged(currency);

            ClearCandlesCommand = ReactiveCommand.Create(ClearCandles);

            SubscribeForCandleSeriesProcessing();
        }

        private async Task SubscribeForCurrencyStateChanged(CurrencyPair currencyPair)
        {
            if (_stockExchange != null)
            {
                _currencyPairsPannel.Block();

                try
                {
                    await SubscribeOrUnsubsribeCandles(currencyPair);
                }
                finally
                {
                    _currencyPairsPannel.Unblock();
                }
            }
        }

        private async Task SubscribeOrUnsubsribeCandles(CurrencyPair currencyPair)
        {
            if (currencyPair.IsSubscribed)
            {
                await _stockExchange.SubscribeCandlesAsync(currencyPair.Name, 60);
            }
            else
            {
                await _stockExchange.UnsubscribeCandlesAsync(currencyPair.Name);
            }
        }

        private void SubscribeForCandleSeriesProcessing()
        {
            _stockExchange.CandleSeriesProcessing += candle => Candles.Push(candle);
        }
        private void ClearCandles()
        {
            Candles.Clear();
        }
    }
}
