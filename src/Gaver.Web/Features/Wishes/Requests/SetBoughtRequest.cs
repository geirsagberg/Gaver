using System.Security.Claims;
using Gaver.Web.CrossCutting;
using Gaver.Web.Features.Wishes.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class SetBoughtRequest : IRequest<SharedWishModel>, IWishListRequest
    {
        public bool IsBought { get; set; }

        [JsonIgnore]
        public int WishId { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
