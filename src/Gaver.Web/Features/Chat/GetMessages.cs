using System.Linq;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.CrossCutting;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Chat
{
    public class GetMessagesRequest : IRequest<ChatModel>, IWishListRequest
    {
        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }

    public class GetMessagesHandler : IRequestHandler<GetMessagesRequest, ChatModel>
    {
        private readonly IMapperService mapper;
        private readonly GaverContext context;

        public GetMessagesHandler(IMapperService mapper, GaverContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public ChatModel Handle(GetMessagesRequest message)
        {
            var messages = context.Set<ChatMessage>()
                .Where(cm => cm.WishListId == message.WishListId)
                .ProjectTo<ChatMessageModel>(mapper.MapperConfiguration).ToList();

            return new ChatModel {
                Messages = messages
            };
        }
    }
}