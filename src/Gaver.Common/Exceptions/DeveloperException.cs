using System;

namespace Gaver.Common.Exceptions
{
    public class DeveloperException : Exception
    {
        public DeveloperException(string message) : base(message)
        {
        }
    }
}
