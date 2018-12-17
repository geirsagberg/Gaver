using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Wishes.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{

    public class AddWishRequest : IRequest<WishModel>, IAuthenticatedRequest
    {
        [Required]
        [MinLength(1)]
        public string Title { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
