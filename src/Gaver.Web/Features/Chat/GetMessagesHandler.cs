using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Chat;

public class GetMessagesHandler(IMapperService mapper, GaverContext context) : IRequestHandler<GetMessagesRequest, ChatDto> {
    private readonly GaverContext context = context;
    private readonly IMapperService mapper = mapper;

    public async Task<ChatDto> Handle(GetMessagesRequest message, CancellationToken token = default) {
        var messages = await context.Set<ChatMessage>()
            .Where(cm => cm.WishListId == message.WishListId)
            .OrderBy(cm => cm.Id)
            .ProjectTo<ChatMessageDto>(mapper.MapperConfiguration).ToListAsync(token);

        return new ChatDto {
            Messages = messages
        };
    }
}
