using System.Collections.Generic;

namespace Gaver.Data
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrDefault<T>(this T obj) => EqualityComparer<T>.Default.Equals(obj, default(T));

        public static string ToStringOrEmpty(this object obj) => obj == null ? "" : obj.ToString();
    }
}