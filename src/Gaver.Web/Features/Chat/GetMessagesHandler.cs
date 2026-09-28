using Gaver.Data;
using Gaver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Chat;

public class GetMessagesHandler(GaverContext context) : IRequestHandler<GetMessagesRequest, ChatDto> {
    public async Task<ChatDto> Handle(GetMessagesRequest message, CancellationToken token = default) {
        var messages = await context.Set<ChatMessage>()
            .Where(cm => cm.WishListId == message.WishListId)
            .OrderBy(cm => cm.Id)
            .Select(ChatMappings.ChatMessageProjection).ToListAsync(token);

        return new ChatDto {
            Messages = messages
        };
    }
}
