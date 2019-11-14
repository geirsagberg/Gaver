using System.Diagnostics.CodeAnalysis;

namespace Gaver.Data
{
    public static class ObjectExtensions
    {
        [return: NotNullIfNotNull("obj")]
        public static string ToStringOrEmpty(this object? obj) => obj == null ? "" : obj.ToString();
    }
}
