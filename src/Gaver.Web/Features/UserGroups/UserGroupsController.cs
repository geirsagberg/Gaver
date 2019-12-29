using System.Threading.Tasks;
using HybridModelBinding;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.UserGroups
{
    public class UserGroupsController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public UserGroupsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<UserGroupsDto> GetMyUserGroups()
        {
            return mediator.Send(new GetMyUserGroupsRequest());
        }

        [HttpPost]
        public Task<UserGroupDto> CreateUserGroup(CreateUserGroupRequest request)
        {
            return mediator.Send(request);
        }

        [HttpPatch("{userGroupId:int}")]
        public Task UpdateUserGroup([FromHybrid] UpdateUserGroupRequest request)
        {
            return mediator.Send(request);
        }
    }
}