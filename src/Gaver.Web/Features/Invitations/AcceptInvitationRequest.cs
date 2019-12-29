using System;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Users;
using MediatR;

namespace Gaver.Web.Features.Invitations
{
    public class AcceptInvitationRequest : IRequest<UserDto>, IAuthenticatedRequest
    {
        public Guid Token { get; }

        public AcceptInvitationRequest(Guid token)
        {
            Token = token;
        }

        public int UserId { get; set; }
    }
}
