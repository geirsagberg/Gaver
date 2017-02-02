using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class CheckSharedListAccessRequest : IRequest<ListAccessStatus>
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int WishListId { get; set; }
    }
}