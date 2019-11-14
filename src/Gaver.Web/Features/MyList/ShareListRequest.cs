using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.MyList
{
    public class ShareListRequest : IRequest, IAuthenticatedRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string[] Emails { get; set; } = new string[0];

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
