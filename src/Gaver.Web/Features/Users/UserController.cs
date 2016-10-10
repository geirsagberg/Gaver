using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly LogInHandler _handler;

        public UserController(LogInHandler handler)
        {
            _handler = handler;
        }

        [HttpPost("LogIn")]
        public async Task<UserModel> LogIn(LogInRequest request)
        {
            var userModel = _handler.Handle(request);
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