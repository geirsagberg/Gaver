using System;
using Gaver.Web.Features.Users;

namespace Gaver.Web.Features.Chat
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }
        public UserDto User { get; set; }
    }
}
