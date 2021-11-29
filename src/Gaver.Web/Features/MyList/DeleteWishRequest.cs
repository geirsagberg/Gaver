using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.MyList;

public class DeleteWishRequest : IRequest<DeleteWishResponse>, IMyWishRequest
{
    [HybridBindProperty(Source.Route)]
    [JsonIgnore]
    public int WishId { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
}