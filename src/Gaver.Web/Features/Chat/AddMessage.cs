using System.ComponentModel.DataAnnotations;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;
using Gaver.Web.Features.LiveUpdates;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Chat
{
    public class AddMessageRequest
    {
        [Required]
        [MinLength(1)]
        public string Text { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }

    public class AddMessageHandler : IRequestHandler<AddMessageRequest, ChatMessageModel>
    {
        private readonly IMapperService mapper;
        private readonly GaverContext context;
        private readonly ClientNotifier _clientNotifier;

        public AddMessageHandler(IMapperService mapper, GaverContext context, ClientNotifier clientNotifier)
        {
            this.mapper = mapper;
            this.context = context;
            _clientNotifier = clientNotifier;
        }

        public ChatMessageModel Handle(AddMessageRequest request)
        {
            var chatMessage = new ChatMessage
            {
                Text = request.Text,
                UserId = request.UserId,
                WishListId = request.WishListId
            };
            context.Add(chatMessage);
            context.SaveChanges();
            chatMessage.User = context.GetOrDie<User>(request.UserId);
            _clientNotifier.RefreshListAsync(request.WishListId, request.UserId);
            return mapper.Map<ChatMessageModel>(chatMessage);
        }
    }
}