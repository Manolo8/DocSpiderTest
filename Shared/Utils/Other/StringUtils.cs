using System.Text.RegularExpressions;

namespace Shared.Utils.Other
{
    public static class StringUtils
    {
        private static Regex regex = new Regex(@"[^\d]");

        public static string OnlyNumbers(string str)
        {
            return regex.Replace(str, "");
        }
    }
}