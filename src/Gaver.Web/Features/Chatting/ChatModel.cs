using System.Collections.Generic;

namespace Gaver.Web.Features.Chatting
{
    public class ChatModel
    {
        public IList<ChatMessageModel> Messages { get; set; } = new List<ChatMessageModel>();
    }
}