using System.Net;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Common.Extensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.UserGroups;

public class UserGroupHandler : IRequestHandler<GetMyUserGroupsRequest, UserGroupsDto>,
    IRequestHandler<CreateUserGroupRequest, UserGroupDto>,
    IRequestHandler<UpdateUserGroupRequest>,
    IRequestHandler<DeleteUserGroupRequest>
{
    private readonly GaverContext context;
    private readonly IMapperService mapperService;

    public UserGroupHandler(GaverContext context, IMapperService mapperService)
    {
        this.context = context;
        this.mapperService = mapperService;
    }

    public async Task<UserGroupDto> Handle(CreateUserGroupRequest request, CancellationToken cancellationToken)
    {
        var user = await context.GetOrDieAsync<User>(request.UserId);
        var userGroup = new UserGroup {
            Name = request.Name,
            CreatedByUser = user,
            UserGroupConnections = new List<UserGroupConnection> {
                new() { UserId = request.UserId }
            }.Concat(request.UserIds.Select(userId => new UserGroupConnection {
                UserId = userId
            })).ToList()
        };
        context.Add(userGroup);
        await context.SaveChangesAsync(cancellationToken);
        return mapperService.Map<UserGroupDto>(userGroup);
    }

    public async Task<Unit> Handle(DeleteUserGroupRequest request, CancellationToken cancellationToken)
    {
        var userGroup =
            await context.UserGroups.SingleOrDefaultAsync(ug => ug.Id == request.UserGroupId && ug.UserGroupConnections.Any(c => c.UserId == request.UserId), cancellationToken) ?? throw new FriendlyException("Finner ikke oppgitt gruppe");


        if (userGroup.CreatedByUserId != request.UserId) throw new FriendlyException("Du kan ikke slette grupper du ikke har opprettet");

        context.Remove(userGroup);
        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    public async Task<UserGroupsDto> Handle(GetMyUserGroupsRequest request, CancellationToken cancellationToken)
    {
        var groups = await context.UserGroupConnections.Where(c => c.UserId == request.UserId)
            .Select(u => u.UserGroup)
            .ProjectTo<UserGroupDto>(mapperService.MapperConfiguration).ToListAsync(cancellationToken);
        return new UserGroupsDto {
            UserGroups = groups
        };
    }

    public async Task<Unit> Handle(UpdateUserGroupRequest request, CancellationToken cancellationToken)
    {
        var userGroup = await context.UserGroups.Include(ug => ug.UserGroupConnections)
                .SingleOrDefaultAsync(c =>
                        c.Id == request.UserGroupId &&
                        c.UserGroupConnections.Any(c2 => c2.UserId == request.UserId),
                    cancellationToken) ??
            throw new HttpException(HttpStatusCode.NotFound,
                "Finner ingen av dine grupper med ID " + request.UserGroupId);

        if (request.Name != null) userGroup.Name = request.Name;

        if (request.UserIds != null) {
            if (request.UserId.NotIn(request.UserIds))
                throw new FriendlyException("Du kan ikke fjerne deg selv fra en gruppe!");
            userGroup.UserGroupConnections = request.UserIds.Select(userId => new UserGroupConnection {
                UserGroupId = request.UserGroupId,
                UserId = userId
            }).ToList();
        }

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
