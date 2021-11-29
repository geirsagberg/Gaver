using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Users;

public class FriendsHandler : IRequestHandler<GetFriendsRequest, List<UserDto>>
{
    private readonly GaverContext context;
    private readonly IMapperService mapperService;

    public FriendsHandler(GaverContext context, IMapperService mapperService)
    {
        this.context = context;
        this.mapperService = mapperService;
    }

    public async Task<List<UserDto>> Handle(GetFriendsRequest request, CancellationToken cancellationToken)
    {
        var users = await context.UserFriendConnections.Where(u => u.UserId == request.UserId).Select(u => u.Friend)
            .ProjectTo<UserDto>(mapperService.MapperConfiguration).ToListAsync(cancellationToken);

        return users;
    }
}