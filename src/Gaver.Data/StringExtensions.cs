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

        public static bool IsNullOrEmpty(this string str) {
            return string.IsNullOrEmpty(str);
        }

        public static int ToInt(this string str) {
            return int.Parse(str);
        }
    }
}
