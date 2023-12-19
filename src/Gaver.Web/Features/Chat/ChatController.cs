using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Chat;

public class ChatController(IMediator mediator) : GaverControllerBase {
    private readonly IMediator mediator = mediator;

    [HttpPost("{wishListId:int}")]
    public Task<ChatMessageDto> AddMessage(AddMessageRequest request) => mediator.Send(request);

    [HttpGet("{wishListId:int}")]
    public Task<ChatDto> GetMessages(GetMessagesRequest request) => mediator.Send(request);
}
