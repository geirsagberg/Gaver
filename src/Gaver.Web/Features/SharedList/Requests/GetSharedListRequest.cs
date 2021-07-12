using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.SharedList.Requests
{
    public class GetSharedListRequest : IRequest<SharedListDto>, ISharedListRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int WishListId { get; init; }
    }
}
