using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.MyList
{
    public class SetWishesOrderRequest : IRequest, IAuthenticatedRequest
    {
        public int[] WishesOrder { get; set; } = new int[0];

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
