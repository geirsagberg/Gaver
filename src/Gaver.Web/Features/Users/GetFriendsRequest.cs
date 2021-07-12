using System.Collections.Generic;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Users
{
    public class GetFriendsRequest : IRequest<List<UserDto>>, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }
}
