using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Chat;

public class AddMessageHandler(GaverContext context, IClientNotifier clientNotifier) : IRequestHandler<AddMessageRequest, ChatMessageDto> {
    public async Task<ChatMessageDto> Handle(AddMessageRequest request, CancellationToken token = default) {
        var userId = request.UserId;
        var chatMessage = new ChatMessage {
            Text = request.Text,
            UserId = userId,
            WishListId = request.WishListId
        };
        context.Add(chatMessage);
        await context.SaveChangesAsync(token);

        var chatMessageModel = ChatMappings.ToDto(chatMessage);
        await clientNotifier.MessageAdded(request.WishListId, chatMessageModel);
        return chatMessageModel;
    }
}
