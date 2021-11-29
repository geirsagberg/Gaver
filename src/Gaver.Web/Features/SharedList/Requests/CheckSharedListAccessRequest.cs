using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.SharedList.Requests;

public class CheckSharedListAccessRequest : IRequest<ListAccessStatus>, IAuthenticatedRequest
{
    [JsonIgnore]
    public int UserId { get; set; }

    [JsonIgnore]
    [HybridBindProperty(Source.Route)]
    public int WishListId { get; init; }
}