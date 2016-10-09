using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public async Task<UserModel> LogIn(LogInRequest request)
        {
            var userModel = mediator.Send(request);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, userModel.Name),
                new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString())
            }, CookieAuthenticationDefaults.AuthenticationScheme));
            await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
            return userModel;
        }

        [HttpPost("LogOut")]
        public async Task LogOut()
        {
            await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}