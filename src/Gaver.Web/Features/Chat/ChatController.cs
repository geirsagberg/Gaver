using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Chat
{
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : GaverControllerBase
    {
        private readonly AddMessageHandler addMessageHandler;
        private readonly GetMessagesHandler getMessagesHandler;

        public ChatController(AddMessageHandler addMessageHandler, GetMessagesHandler getMessagesHandler)
        {
            this.addMessageHandler = addMessageHandler;
            this.getMessagesHandler = getMessagesHandler;
        }

        [HttpPost("{listId:int}")]
        public ChatMessageModel AddMessage(int listId, AddMessageRequest request)
        {
            request.WishListId = listId;
            request.UserId = UserId;
            return addMessageHandler.Handle(request);
        }

        [HttpGet("{listId:int}")]
        public ChatModel GetMessages(int listId)
        {
            return getMessagesHandler.Handle(new GetMessagesRequest{WishListId = listId});
        }
    }
}