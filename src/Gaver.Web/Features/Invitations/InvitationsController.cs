using System;
using System.Threading.Tasks;
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
        public Task<InvitationStatusModel> GetInvitationStatus(Guid token)
        {
            return mediator.Send(new GetInvitationStatusRequest(token));
        }

        [HttpPost("{token:guid}/Accept")]
        public Task<AcceptInvitationResponse> AcceptInvitation(Guid token)
        {
            return mediator.Send(new AcceptInvitationRequest(token));
        }
    }
}
