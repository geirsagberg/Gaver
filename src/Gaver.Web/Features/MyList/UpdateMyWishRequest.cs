using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.MyList
{
    public class UpdateMyWishRequest : IRequest, IMyWishRequest
    {
        [MinLength(1)]
        public string Title { get; set; }

        [Url]
        public string Url { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int WishId { get; set; }
    }
}
