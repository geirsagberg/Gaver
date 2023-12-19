using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Chat;

public class AddMessageHandler(IMapperService mapper, GaverContext context, IClientNotifier clientNotifier) : IRequestHandler<AddMessageRequest, ChatMessageDto> {
    private readonly IClientNotifier clientNotifier = clientNotifier;
    private readonly GaverContext context = context;
    private readonly IMapperService mapper = mapper;

    public async Task<ChatMessageDto> Handle(AddMessageRequest request, CancellationToken token = default) {
        var userId = request.UserId;
        var chatMessage = new ChatMessage {
            Text = request.Text,
            UserId = userId,
            WishListId = request.WishListId
        };
        context.Add(chatMessage);
        await context.SaveChangesAsync(token);

        var chatMessageModel = mapper.Map<ChatMessageDto>(chatMessage);
        await clientNotifier.MessageAdded(request.WishListId, chatMessageModel);
        return chatMessageModel;
    }
}
