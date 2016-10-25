using System;
using System.Collections.Generic;
using System.Linq;

namespace Gaver.Logic.Extensions
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
    }
}