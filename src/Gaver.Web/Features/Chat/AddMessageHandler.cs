using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Chat
{
    public class AddMessageHandler : IRequestHandler<AddMessageRequest, ChatMessageModel>
    {
        private readonly IClientNotifier clientNotifier;
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public AddMessageHandler(IMapperService mapper, GaverContext context, IClientNotifier clientNotifier)
        {
            this.mapper = mapper;
            this.context = context;
            this.clientNotifier = clientNotifier;
        }

        public async Task<ChatMessageModel> Handle(AddMessageRequest request, CancellationToken token = default)
        {
            var userId = request.UserId;
            var chatMessage = new ChatMessage {
                Text = request.Text,
                UserId = userId,
                WishListId = request.WishListId
            };
            context.Add(chatMessage);
            await context.SaveChangesAsync(token);

            var chatMessageModel = mapper.Map<ChatMessageModel>(chatMessage);
            await clientNotifier.MessageAdded(request.WishListId, chatMessageModel);
            return chatMessageModel;
        }
    }
}
