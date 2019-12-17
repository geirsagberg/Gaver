using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.UserGroups
{
    public class GetMyUserGroupsRequest : IRequest<UserGroupsDto>, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }
}