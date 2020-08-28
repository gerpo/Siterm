using System.Threading;

namespace Siterm.Support.Misc
{
    public static class StringExtension
    {
        public static string ToTitleCase(this string str)
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}