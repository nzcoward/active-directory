using System.Text.RegularExpressions;

namespace Arc.ActiveDirectory.Web
{
    public static class StringExtensions
    {
        public static int Match(this string target, Regex regex)
        {
            return regex.IsMatch(target) ? 1 : 0;
        }
    }
}