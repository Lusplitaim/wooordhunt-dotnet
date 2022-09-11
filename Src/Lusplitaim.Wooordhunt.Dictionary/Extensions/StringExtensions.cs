
namespace Lusplitaim.Wooordhunt
{
    internal static class StringExtensions
    {
        internal static string RemoveDash(this string str)
        {
            return str.Replace("—", string.Empty).Trim();
        }

        internal static string RemoveArrow(this string str)
        {
            return str.Replace("↓", string.Empty).Trim();
        }
    }
}
