using ReactiveUI;
using StockExchangeCore.Abstract;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BitfinexUI.ViewModels
{
    public partial class PortfolioViewModel : PageViewModel
    {
        private readonly IStockExchangeRestConnector _stockExchange;

        public ObservableCollection<PortfolioBalance> Balances { get; } = new ObservableCollection<PortfolioBalance>();

        private readonly double _btcBalance = 1;
        private readonly double _xrpBalance = 15000;
        private readonly double _xmrBalance = 50;
        private readonly double _dashBalance = 30;

        public PortfolioViewModel(string header, IStockExchangeRestConnector stockExchange) : base(header)
        {
            _stockExchange = stockExchange;

            CalculateBalancesAsync().ConfigureAwait(false);

            CalculateTotalBalanceCommand = ReactiveCommand.Create(CalculateBalancesAsync);
        }

        public ICommand CalculateTotalBalanceCommand { get; }

        private async Task CalculateBalancesAsync()
        {
            var btcToUsdt = await GetTickerLastPriceAsync("BTCUSD");
            var xrpToUsdt = await GetTickerLastPriceAsync("XRPUSD");
            var xmrToUsdt = await GetTickerLastPriceAsync("XMRUSD");
            var dashToUsdt = await GetTickerLastPriceAsync("DSHUSD");

            double totalUsdt = (_btcBalance * btcToUsdt) +
                                (_xrpBalance * xrpToUsdt) +
                                (_xmrBalance * xmrToUsdt) +
                                (_dashBalance * dashToUsdt);

            var totalBtc = totalUsdt / btcToUsdt;
            var totalXrp = totalUsdt / xrpToUsdt;
            var totalXmr = totalUsdt / xmrToUsdt;
            var totalDash = totalUsdt / dashToUsdt;

            Balances.Clear();

            Balances.Add(new PortfolioBalance { Currency = "USDT", Balance = totalUsdt });
            Balances.Add(new PortfolioBalance { Currency = "BTC", Balance = totalBtc });
            Balances.Add(new PortfolioBalance { Currency = "XRP", Balance = totalXrp });
            Balances.Add(new PortfolioBalance { Currency = "XMR", Balance = totalXmr });
            Balances.Add(new PortfolioBalance { Currency = "DASH", Balance = totalDash });
        }

        private async Task<double> GetTickerLastPriceAsync(string pair)
        {
            var ticker = await _stockExchange.GetTickerAsync(pair);

            return ticker.LastPrice;
        }
    }
}
