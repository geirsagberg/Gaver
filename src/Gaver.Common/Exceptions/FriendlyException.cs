using System;

namespace Gaver.Common.Exceptions
{
    public class FriendlyException : Exception
    {
        public int EventId { get; }

        public FriendlyException(int eventId, string message) : base(message)
        {
            EventId = eventId;
        }
    }
}