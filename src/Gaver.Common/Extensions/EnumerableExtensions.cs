using System;
using System.Collections.Generic;
using System.Linq;

namespace Gaver.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static T SingleOrThrow<T>(this IEnumerable<T> enumerable, Exception exception)
        {
            var result = enumerable.SingleOrDefault();
            if (result == null)
            {
                throw exception;
            }
            return result;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable == null || !enumerable.Any();

        public static string ToJoinedString<T>(this IEnumerable<T> enumerable, string separator = ", ")
        {
            return string.Join(separator, enumerable);
        }
    }
}