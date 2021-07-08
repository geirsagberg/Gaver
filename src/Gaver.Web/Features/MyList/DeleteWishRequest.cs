using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.MyList
{
    public class DeleteWishRequest : IRequest<DeleteWishResponse>, IMyWishRequest
    {
        [JsonIgnore]
        public int WishId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
