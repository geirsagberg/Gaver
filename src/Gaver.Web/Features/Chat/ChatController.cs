using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Chat;

public class ChatController : GaverControllerBase
{
    private readonly IMediator mediator;

    public ChatController(IMediator mediator) => this.mediator = mediator;

    [HttpPost("{wishListId:int}")]
    public Task<ChatMessageDto> AddMessage(AddMessageRequest request) => mediator.Send(request);

    [HttpGet("{wishListId:int}")]
    public Task<ChatDto> GetMessages(GetMessagesRequest request) => mediator.Send(request);
}