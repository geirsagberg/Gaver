using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Users
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<CurrentUserModel> GetUserInfo()
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
