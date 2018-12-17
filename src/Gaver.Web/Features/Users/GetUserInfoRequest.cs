using System.Security.Claims;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Users
{
    public class GetUserInfoRequest : IRequest<CurrentUserModel>, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }
}
