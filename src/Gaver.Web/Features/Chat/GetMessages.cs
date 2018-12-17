using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using MediatR;

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

        public Task<ChatModel> Handle(GetMessagesRequest message, CancellationToken token = default)
        {
            var messages = context.Set<ChatMessage>()
                .Where(cm => cm.WishListId == message.WishListId)
                .ProjectTo<ChatMessageModel>(mapper.MapperConfiguration).ToList();

            return Task.FromResult(new ChatModel {
                Messages = messages
            });
        }
    }
}
