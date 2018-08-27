namespace iOrder.dataaccess.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        public static bool IsNotNullOrEmpty(this string source)
        {
            return !source.IsNullOrEmpty();
        }

        public static string Join(this IEnumerable<string> source, string seperator)
        {
            var result = string.Join(seperator, source);
            return result;
        }
    }
}
