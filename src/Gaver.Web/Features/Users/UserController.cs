using System.Threading.Tasks;
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

        [HttpPost]
        public Task UpdateUserInfo()
        {
            return mediator.Send(new UpdateUserInfoRequest());
        }
    }
}
