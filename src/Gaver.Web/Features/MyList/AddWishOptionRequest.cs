using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Shared.Models;
using MediatR;

namespace Gaver.Web.Features.MyList
{
    public class AddWishOptionRequest : IRequest<WishOptionDto>, IMyWishRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int WishId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
