using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.SharedList.Requests
{
    public class GetSharedListRequest : IRequest<SharedListDto>, ISharedListRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
    }
}
