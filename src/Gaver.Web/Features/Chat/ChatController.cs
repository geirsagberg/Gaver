using System.Threading.Tasks;
using HybridModelBinding;
using MediatR;
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

        [HttpPost("{wishListId:int}")]
        public Task<ChatMessageDto> AddMessage([FromHybrid] AddMessageRequest request)
        {
            return mediator.Send(request);
        }

        [HttpGet("{wishListId:int}")]
        public Task<ChatDto> GetMessages(int wishListId)
        {
            return mediator.Send(new GetMessagesRequest {
                WishListId = wishListId
            });
        }
    }
}
