using Gaver.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Users;

public class FriendsHandler(GaverContext context) : IRequestHandler<GetFriendsRequest, List<UserDto>> {
    public async Task<List<UserDto>> Handle(GetFriendsRequest request, CancellationToken cancellationToken) {
        var users = await context.UserFriendConnections.Where(u => u.UserId == request.UserId).Select(u => u.Friend!)
            .Select(UserMappings.UserDtoProjection).ToListAsync(cancellationToken);

        return users;
    }
}
