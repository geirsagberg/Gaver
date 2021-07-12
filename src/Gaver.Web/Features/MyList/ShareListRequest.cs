using System;
using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.MyList
{
    public class ShareListRequest : IRequest, IAuthenticatedRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string[] Emails { get; set; } = Array.Empty<string>();

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
