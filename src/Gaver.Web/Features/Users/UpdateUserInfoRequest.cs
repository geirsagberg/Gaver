using System.Security.Claims;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.Users
{
    public class UpdateUserInfoRequest : IRequest, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }
}
