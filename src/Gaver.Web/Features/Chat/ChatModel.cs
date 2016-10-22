using System.Collections.Generic;

namespace Gaver.Web.Features.Chat
{
    public class ChatModel
    {
        public IList<ChatMessageModel> Messages { get; set; } = new List<ChatMessageModel>();
    }
}