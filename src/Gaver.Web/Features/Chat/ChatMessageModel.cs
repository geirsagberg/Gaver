using System;

namespace Gaver.Web.Features.Chat
{
    public class ChatMessageModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTimeOffset Created { get; set; }
        public UserModel User { get; set; }
    }
}