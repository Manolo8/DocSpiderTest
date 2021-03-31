using System.Globalization;

namespace Shared {
    public static class Configuration {
        public static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("pt-BR");

        public static string MonthName(int month) {
            return Culture.DateTimeFormat.GetMonthName(month);
        }
    }
}