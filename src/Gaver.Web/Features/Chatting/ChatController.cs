using System.Linq;
using System.Security.Claims;
using Gaver.Data;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Chatting
{
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IMediator mediator;

        public ChatController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("{listId:int}")]
        public ChatMessageModel AddMessage(int listId, AddMessageRequest request)
        {
            request.WishListId = listId;
            request.UserId = User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value.ToInt();
            return mediator.Send(request);
        }

        [HttpGet("{listId:int}")]
        public ChatModel GetMessages(int listId)
        {
            return mediator.Send(new GetMessagesRequest{WishListId = listId});
        }
    }
}