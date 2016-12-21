using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Users
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : GaverControllerBase
    {
        private readonly UserHandler userHandler;
        public UserController(UserHandler userHandler)
        {
            this.userHandler = userHandler;
        }

        [HttpGet]
        public Task<LoginUserModel> GetUserInfo()
        {
            return userHandler.HandleAsync(new GetUserInfoRequest{
                UserId = UserId
            });
        }
    }
}