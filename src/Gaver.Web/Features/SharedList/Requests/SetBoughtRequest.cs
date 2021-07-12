using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.SharedList.Requests
{
    public class SetBoughtRequest : IRequest<SharedWishDto>, ISharedListRequest
    {
        public bool IsBought { get; set; }

        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int WishId { get; set; }

        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
