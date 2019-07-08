using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Chat
{
    public class GetMessagesHandler : IRequestHandler<GetMessagesRequest, ChatModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public GetMessagesHandler(IMapperService mapper, GaverContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ChatModel> Handle(GetMessagesRequest message, CancellationToken token = default)
        {
            var messages = await context.Set<ChatMessage>()
                .Where(cm => cm.WishListId == message.WishListId)
                .OrderBy(cm => cm.Id)
                .ProjectTo<ChatMessageModel>(mapper.MapperConfiguration).ToListAsync();

            return new ChatModel {
                Messages = messages
            };
        }
    }
}
