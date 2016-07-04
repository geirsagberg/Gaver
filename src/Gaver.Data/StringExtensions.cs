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
    }
}
