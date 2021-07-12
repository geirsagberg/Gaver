using System;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Users;
using HybridModelBinding;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Invitations
{
    public class AcceptInvitationRequest : IRequest<UserDto>, IAuthenticatedRequest
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public Guid Token { get; init; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
