using Gaver.Web.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Invitations;

public class InvitationsController(IMediator mediator) : GaverControllerBase {
    [HttpGet("{token:guid}/Status")]
    public Task<InvitationStatusDto> GetInvitationStatus(GetInvitationStatusRequest request) => mediator.Send(request);

    [HttpPost("{token:guid}/Accept")]
    public Task<UserDto> AcceptInvitation(AcceptInvitationRequest request) => mediator.Send(request);
}
