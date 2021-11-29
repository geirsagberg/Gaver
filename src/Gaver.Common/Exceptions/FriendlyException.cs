using System;

namespace Gaver.Common.Exceptions;

public class FriendlyException : Exception
{
    public FriendlyException(string message) : base(message)
    {
    }
}