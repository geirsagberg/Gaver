using System;
using Gaver.Web.Contracts;
using Gaver.Web.Models;
using MediatR;

namespace Gaver.Web.Features.Invitations
{
    public class AcceptInvitationRequest : IRequest<InvitationModel>, IAuthenticatedRequest
    {
        public Guid Token { get; }

        public AcceptInvitationRequest(Guid token)
        {
            Token = token;
        }

        public int UserId { get; set; }
    }
}
