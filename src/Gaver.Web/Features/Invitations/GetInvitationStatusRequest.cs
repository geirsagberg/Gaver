using System;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Invitations
{
    public class GetInvitationStatusRequest : IRequest<InvitationStatusModel>, IAuthenticatedRequest
    {
        public GetInvitationStatusRequest(Guid token)
        {
            Token = token;
        }

        public Guid Token { get; }
        public int UserId { get; set; }
    }
}