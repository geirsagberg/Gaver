using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.MyList;

public class AddWishRequest : IRequest<WishDto>, IAuthenticatedRequest
{
    [Required]
    [MinLength(1)]
    [MaxLength(255)]
    public string Title { get; set; } = "";

    [MaxLength(255)]
    public string? Url { get; set; }

    [JsonIgnore]
    public int UserId { get; set; }
}