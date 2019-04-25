using Gaver.Web.Contracts;
using Gaver.Web.Features.Wishes.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class SetUrlRequest : IRequest<WishModel>, IMyWishRequest
    {
        public string Url { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int WishId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
