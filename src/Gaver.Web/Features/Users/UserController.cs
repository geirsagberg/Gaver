using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Data;
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

        [HttpGet("/users")]
        public Task<List<UserDto>> GetUsers()
        {
            return mediator.Send(new GetUsersRequest());
        }

        [HttpPost]
        public Task UpdateUserInfo()
        {
            return mediator.Send(new UpdateUserInfoRequest());
        }
    }

    public class GetUsersRequest : IRequest<List<UserDto>>, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }

    public class UsersHandler : IRequestHandler<GetUsersRequest, List<UserDto>>
    {
        private readonly GaverContext context;

        public UsersHandler(GaverContext context)
        {
            this.context = context;
        }

        public async Task<List<UserDto>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            // var users = context.Users.Where(u => u.WishList.Invitations)
        }
    }
}
