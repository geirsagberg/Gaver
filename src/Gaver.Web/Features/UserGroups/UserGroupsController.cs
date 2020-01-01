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
        public Task<UserGroupsDto> GetMyUserGroups() => mediator.Send(new GetMyUserGroupsRequest());

        [HttpPost]
        public Task<UserGroupDto> CreateUserGroup(CreateUserGroupRequest request) => mediator.Send(request);

        [HttpPatch("{userGroupId:int}")]
        public Task UpdateUserGroup([FromHybrid] UpdateUserGroupRequest request) => mediator.Send(request);

        [HttpDelete("{userGroupId:int}")]
        public Task DeleteUserGroup([FromHybrid] DeleteUserGroupRequest request) => mediator.Send(request);
    }
}
