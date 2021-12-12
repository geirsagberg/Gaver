using Gaver.Web.Contracts;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.MyList;

public class SetWishesOrderRequest : IRequest, IAuthenticatedRequest
{
    public int[] WishesOrder { get; set; } = Array.Empty<int>();

    [JsonIgnore]
    public int UserId { get; set; }
}