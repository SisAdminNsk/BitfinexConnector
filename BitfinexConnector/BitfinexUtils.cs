using System.Reflection;

namespace BitfinexConnector
{
    internal static class BitfinexUtils
    {
        public static string ConvertPeriodToTimeFrame(int periodInSec)
        {
            return periodInSec switch
            {
                60 => "1m",      // 1 минута
                300 => "5m",     // 5 минут
                900 => "15m",    // 15 минут
                1800 => "30m",   // 30 минут
                3600 => "1h",    // 1 час
                10800 => "3h",   // 3 часа
                21600 => "6h",   // 6 часов
                43200 => "12h",  // 12 часов
                86400 => "1D",   // 1 день
                604800 => "1W",  // 1 неделя
                1209600 => "14D",// 14 дней
                2592000 => "1M", // 1 месяц (30 дней)

                _ => throw new ArgumentException("Неподдерживаемый период." +
                " Доступные значения: 60, 300, 900, 1800, 3600, 10800, 21600, 43200, 86400, 604800, 1209600, 2592000 секунд.")
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
