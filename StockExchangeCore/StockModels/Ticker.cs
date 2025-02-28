namespace StockExchangeCore.StockModels
{
    public class Ticker
    {
        /// <summary>
        /// Валютная пара
        /// </summary>
        public string Pair { get; set; }

        /// <summary>
        /// Цена последней наивысшей ставки
        /// </summary>
        public double Bid { get; set; }

        /// <summary>
        /// Сумма 25 размеров самой высокой ставки
        /// </summary>
        public double BidSize { get; set; }

        /// <summary>
        /// Цена последней самой низкой цены
        /// </summary>
        public double Ask { get; set; }

        /// <summary>
        /// Сумма 25 наименьших размеров спроса
        /// </summary>
        public double AskSize { get; set; }

        /// <summary>
        /// Сумма, на которую изменилась последняя цена со вчерашнего дня
        /// </summary>
        public double DailyChange { get; set; }

        /// <summary>
        /// Относительное изменение цены со вчерашнего дня (*100 для процентного изменения)
        /// </summary>
        public double DailyChangeRelative { get; set; }

        /// <summary>
        /// Цена последней сделки
        /// </summary>
        public double LastPrice { get; set; }

        /// <summary>
        /// Дневной объем
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Дневной максимум
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Дневной минимум
        /// </summary>
        public double Low { get; set; }
    }
}
