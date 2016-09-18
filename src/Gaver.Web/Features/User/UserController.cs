using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("LogIn")]
        public UserModel LogIn(LogInRequest request)
        {
            var userModel = mediator.Send(request);
            return userModel;
        }
    }
}