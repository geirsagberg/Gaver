using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Gaver.Web.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Users
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : GaverControllerBase
    {
        private readonly IMediator mediator;

        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<UserModel> GetUserInfo([FromQuery] string accessToken)
        {
            var providerId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (providerId == null) {
                throw new HttpException(HttpStatusCode.Unauthorized);
            }
            var request = new GetUserInfoRequest {
                AccessToken = accessToken,
                ProviderId = providerId
            };

            return mediator.Send(request);
        }
    }
}