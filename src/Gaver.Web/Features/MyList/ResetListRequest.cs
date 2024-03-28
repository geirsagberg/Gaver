using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class ResetListRequest : IRequest, IAuthenticatedRequest {
    [JsonIgnore]
    public int UserId { get; set; }

    public HashSet<int> KeepWishes { get; set; } = [];
}
