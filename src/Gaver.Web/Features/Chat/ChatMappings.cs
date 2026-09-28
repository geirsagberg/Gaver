using System.Linq.Expressions;
using Gaver.Data.Entities;

namespace Gaver.Web.Features.Chat;

public static class ChatMappings {
    public static Expression<Func<ChatMessage, ChatMessageDto>> ChatMessageProjection { get; } = message =>
        new ChatMessageDto {
            Id = message.Id,
            Text = message.Text,
            Created = message.Created,
            User = message.User == null
                ? null
                : new ChatUserDto {
                    Id = message.User.Id,
                    Name = message.User.Name,
                    PictureUrl = message.User.PictureUrl
                }
        };

    private static Func<ChatMessage, ChatMessageDto> MapChatMessage { get; } = ChatMessageProjection.Compile();

    public static ChatMessageDto ToDto(ChatMessage message) => MapChatMessage(message);
}
