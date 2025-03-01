using System.Reflection;

namespace BitfinexConnector
{
    public static class BitfinexUtils
    {
        public static int ConvertPeriodToTimeFrame(string periodString)
        {
            var availablePeriods = GetAvailablePeriodInSec();

            if (!availablePeriods.ContainsKey(periodString))
            {
                throw new ArgumentException("Неподдерживаемый период." +
              " Доступные значения: 1m, 5m, 15m, 30m, 1h, 3h, 6h, 12h, 1D, 1W, 14D, 1M.");
            }

            return availablePeriods[periodString];
        }

        public static string ConvertPeriodToTimeFrame(int periodInSec)
        {
            var availablePeriods = GetAvailablePeriodInSec();

            var period = availablePeriods.FirstOrDefault(x => x.Value == periodInSec).Key;

            if (!availablePeriods.ContainsKey(period))
            {
                throw new ArgumentException("Неподдерживаемый период." +
                " Доступные значения: 60, 300, 900, 1800, 3600, 10800, 21600, 43200, 86400, 604800, 1209600, 2592000 секунд.");
            }

            return period;
        }

        public static Dictionary<string, int> GetAvailablePeriodInSec()
        {
            return new Dictionary<string, int>()
            {
                {"1m",60 },
                {"5m",300 },
                {"15m",900 },
                {"30m",1800 }, 
                {"1h",3600 },
                {"3h",10800 },
                {"6h",21600 },
                {"12h", 43200 },
                {"1D",86400 },
                {"1W",604800 },
                {"14D",1209600 },
                {"1M",2592000 }
            };
        }

        public static List<string> GetAvaliableCurrencyPairs()
        {
            return new List<string>()
            {
                "BTCUSD",
                "XRPUSD",
                "XMRUSD",
                "DSHUSD"
            };
        }
        public static string GetCurrentMethodFullName()
        {
            var methodInfo = MethodBase.GetCurrentMethod();

            string className = methodInfo.DeclaringType?.FullName ?? "UnknownClass";
            string methodName = methodInfo.Name;

            return $"{className}.{methodName}";
        }
    }
}
