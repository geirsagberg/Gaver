using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Gaver.Data
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (str == null || str.Length < 2)
                return str;

            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        public static string ReplaceRegex(this string input, [RegexPattern] string pattern, string replacement,
            RegexOptions options = RegexOptions.None)
        {
            return Regex.Replace(input, pattern, replacement, options);
        }

        /// <summary>
        /// Replace all duplicate whitespace with a single space, and remove whitespace at start or end.
        /// </summary>
        public static string TrimExtraWhitespace(this string str) => str.ReplaceRegex(@"[\s]{2,}", " ").Trim();
    }
}