using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class ShareListRequest : IRequest<ShareListResponse>, IAuthenticatedRequest {
    [JsonIgnore] public int UserId { get; set; }
}

public class ShareListResponse {
    public required string ShareUrl { get; set; }
}
