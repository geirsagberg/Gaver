using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class SetWishesOrderRequest : IRequest, IAuthenticatedRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public int[] WishesOrder { get; set; } = new int[0];
    }
}
