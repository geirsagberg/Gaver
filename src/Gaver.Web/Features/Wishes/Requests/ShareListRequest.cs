using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class ShareListRequest : IRequest, IAuthenticatedRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string[] Emails { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
