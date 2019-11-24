using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.MyList
{
    public class InviteUserRequest : IRequest, IAuthenticatedRequest
    {
        public int UserId { get; set; }
        public int InviteUserId { get; set; }
    }
}
