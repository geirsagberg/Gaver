using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class ShareListRequest : IRequest, IAuthenticatedRequest {
    [Required]
    [MinLength(1)]
    [MaxLength(10)]
    public string[] Emails { get; set; } = Array.Empty<string>();

    [JsonIgnore] public int UserId { get; set; }
}
