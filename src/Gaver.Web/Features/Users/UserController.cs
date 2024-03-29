using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Users;

public class UserController(IMediator mediator) : GaverControllerBase {
    private readonly IMediator mediator = mediator;

    [HttpGet]
    public Task<CurrentUserDto> GetUserInfo() => mediator.Send(new GetUserInfoRequest());

    [HttpGet("/api/Friends")]
    public Task<List<UserDto>> GetFriends() => mediator.Send(new GetFriendsRequest());

    [HttpPost]
    public Task UpdateUserInfo() => mediator.Send(new UpdateUserInfoRequest());
}
