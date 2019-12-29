using System;
using System.Threading.Tasks;
using Gaver.Web.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Invitations
{
    public class InvitationsController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public InvitationsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{token:guid}/Status")]
        public Task<InvitationStatusDto> GetInvitationStatus(Guid token)
        {
            return mediator.Send(new GetInvitationStatusRequest(token));
        }

        [HttpPost("{token:guid}/Accept")]
        public Task<UserDto> AcceptInvitation(Guid token)
        {
            return mediator.Send(new AcceptInvitationRequest(token));
        }
    }
}
