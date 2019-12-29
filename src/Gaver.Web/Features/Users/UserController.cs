using System.Collections.Generic;
using System.Threading.Tasks;
using Gaver.Web.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Users
{
    public class UserController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<CurrentUserDto> GetUserInfo()
        {
            return mediator.Send(new GetUserInfoRequest());
        }

        [HttpGet("/api/Friends")]
        public Task<List<UserDto>> GetFriends()
        {
            return mediator.Send(new GetFriendsRequest());
        }

        [HttpPost]
        public Task UpdateUserInfo()
        {
            return mediator.Send(new UpdateUserInfoRequest());
        }
    }

    public class GetFriendsRequest : IRequest<List<UserDto>>, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }
}
