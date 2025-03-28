using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class UpdateWishRequest : IRequest, IMyWishRequest {
    [MinLength(1)] public string? Title { get; init; }

    public string? Url { get; init; }

    [JsonIgnore] public int UserId { get; set; }

    [HybridBindProperty(Source.Route)]
    [JsonIgnore]
    public int WishId { get; init; }
}
