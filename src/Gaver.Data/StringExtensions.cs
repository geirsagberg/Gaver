using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

using System.Diagnostics.CodeAnalysis;
namespace Gaver.Data
{
    public static class StringExtensions
    {
        [return: NotNullIfNotNull("str")]
        public static string? ToCamelCase(this string? str)
        {
            if (str == null || str.Length < 2)
                return str;

            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        public static bool IsNullOrEmpty(this string? str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNotEmpty([NotNullWhen(true)] this string? str)
        {
            return !string.IsNullOrEmpty(str);
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

        public static bool IsValidEmail(this string str)
        {
            return str != null && EmailRegex.IsMatch(str);
        }

        private static readonly Regex EmailRegex = CreateEmailRegex();

        private static Regex CreateEmailRegex()
        {
            const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            // Set explicit regex match timeout, sufficient enough for email parsing
            // Unless the global REGEX_DEFAULT_MATCH_TIMEOUT is already set
            var matchTimeout = TimeSpan.FromSeconds(2);

            try {
                return new Regex(pattern, options, matchTimeout);
            } catch {
                // Fallback on error
            }

            // Legacy fallback (without explicit match timeout)
            return new Regex(pattern, options);
        }
    }
}
