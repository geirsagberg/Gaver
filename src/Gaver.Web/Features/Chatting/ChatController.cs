using System.Linq;
using System.Security.Claims;
using Gaver.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Chatting
{
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly AddMessageHandler _addMessageHandler;
        private readonly GetMessagesHandler _getMessagesHandler;

        public ChatController(AddMessageHandler addMessageHandler, GetMessagesHandler getMessagesHandler)
        {
            _addMessageHandler = addMessageHandler;
            _getMessagesHandler = getMessagesHandler;
        }

        [HttpPost("{listId:int}")]
        public ChatMessageModel AddMessage(int listId, AddMessageRequest request)
        {
            request.WishListId = listId;
            request.UserId = User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value.ToInt();
            return _addMessageHandler.Handle(request);
        }

        [HttpGet("{listId:int}")]
        public ChatModel GetMessages(int listId)
        {
            return _getMessagesHandler.Handle(new GetMessagesRequest{WishListId = listId});
        }
    }
}