using System;

namespace Gaver.Logic
{
    public class FriendlyException : Exception
    {
        public int EventId { get; set; }

        public FriendlyException(int eventId, string message) : base(message)
        {
            EventId = eventId;
        }
    }
}