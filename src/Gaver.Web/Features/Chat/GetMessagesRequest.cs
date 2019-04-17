using System.Security.Claims;
using Gaver.Web.Contracts;
using Gaver.Web.CrossCutting;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Chat
{
    public class GetMessagesRequest : IRequest<ChatModel>, ISharedListRequest
    {
        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}