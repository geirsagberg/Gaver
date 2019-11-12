using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.SharedList.Requests
{
    public class CheckSharedListAccessRequest : IRequest<ListAccessStatus>, IAuthenticatedRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int WishListId { get; set; }
    }
}
