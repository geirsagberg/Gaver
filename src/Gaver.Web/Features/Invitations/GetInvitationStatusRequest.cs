using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;

namespace Gaver.Web.Features.Invitations;

public class GetInvitationStatusRequest : IRequest<InvitationStatusDto>, IAuthenticatedRequest {
    [JsonIgnore]
    [HybridBindProperty(Source.Route)]
    public Guid Token { get; init; }

    [JsonIgnore] public int UserId { get; set; }
}
