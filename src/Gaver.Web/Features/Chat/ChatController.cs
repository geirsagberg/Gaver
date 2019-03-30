using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Chat
{
    public class ChatController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public ChatController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("{listId:int}")]
        public Task<ChatMessageModel> AddMessage(int listId, AddMessageRequest request)
        {
            request.WishListId = listId;
            return mediator.Send(request);
        }

        [HttpGet("{listId:int}")]
        public Task<ChatModel> GetMessages(int listId)
        {
            return mediator.Send(new GetMessagesRequest {
                WishListId = listId
            });
        }
    }
}
