using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using Gaver.Web.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.MyList
{
    public class AddWishRequest : IRequest<WishModel>, IAuthenticatedRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(64)]
        public string Title { get; set; }

        public string Url { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
