using BitfinexUI.Common;
using StockExchangeCore.Abstract;
using StockExchangeCore.StockModels;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static BitfinexUI.ViewModels.WsViewModel;

namespace BitfinexUI.ViewModels
{
    public class WsTradesViewModel : PageViewModel
    {

        private CurrencyPairsPannelViewModel _currencyPairsPannel;
        public ObservableCollection<CurrencyPair> Currencies { get => _currencyPairsPannel.Currencies; }
        public ObservableStack<Trade> Trades { get; set; } = new();

        private readonly IStockExchangeWsConnector _stockExchange;

        public WsTradesViewModel(string header, IStockExchangeWsConnector stockExchange) : base(header) 
        {
            _stockExchange = stockExchange;

            _currencyPairsPannel = new();

            _currencyPairsPannel.CurrencyPairStateChanged += async currency => await SubscribeForCurrencyStateChanged(currency);

            SubscribeForNewBuyOrSellTrades();
        }

        private async Task SubscribeForCurrencyStateChanged(CurrencyPair currencyPair)
        {
            if (_stockExchange != null)
            {
                _currencyPairsPannel.Block(); 

                try
                {
                    await SubscribeOrUnsubsribeTrades(currencyPair);
                }
                finally
                {
                    _currencyPairsPannel.Unblock(); 
                }
            }
        }

        private async Task SubscribeOrUnsubsribeTrades(CurrencyPair currencyPair)
        {
            if (currencyPair.IsSubscribed)
            {
                await _stockExchange.SubscribeTradesAsync(currencyPair.Name);
            }
            else
            {
                await _stockExchange.UnsubscribeTradesAsync(currencyPair.Name);
            }
        }

        private void SubscribeForNewBuyOrSellTrades()
        {
            _stockExchange.NewBuyTrade += trade => Trades.Push(trade);
            _stockExchange.NewSellTrade += trade => Trades.Push(trade);
        }
    }
}
