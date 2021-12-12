using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Common.Extensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Features.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Invitations;

public class InvitationHandler : IRequestHandler<GetInvitationStatusRequest, InvitationStatusDto>,
    IRequestHandler<AcceptInvitationRequest, UserDto>
{
    private readonly GaverContext context;
    private readonly IMapperService mapperService;

    public InvitationHandler(GaverContext context, IMapperService mapperService)
    {
        this.context = context;
        this.mapperService = mapperService;
    }

    public async Task<UserDto> Handle(AcceptInvitationRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = request.UserId;
        var invitationToken = await CheckInvitationStatus(request.Token, userId);

        var wishList = await context.WishLists.Include(wl => wl.User)
            .SingleAsync(wl => wl.Id == invitationToken.WishListId, cancellationToken);

        var friendId = wishList.UserId;
        var existingConnections = await context.UserFriendConnections.Where(u =>
            u.UserId == userId && u.FriendId == friendId ||
            u.UserId == friendId && u.FriendId == userId).ToListAsync(cancellationToken);

        if (existingConnections.None(c => c.UserId == userId)) {
            context.Add(new UserFriendConnection {
                UserId = userId,
                FriendId = friendId
            });
        }

        if (existingConnections.None(c => c.UserId == friendId)) {
            context.Add(new UserFriendConnection {
                UserId = friendId,
                FriendId = userId
            });
        }

        await context.SaveChangesAsync(cancellationToken);
        var friend = await context.Set<User>().Where(u => u.WishList!.Id == invitationToken.WishListId).ProjectTo<UserDto>(mapperService.MapperConfiguration).SingleAsync(cancellationToken);
        return friend;
    }

    public async Task<InvitationStatusDto> Handle(GetInvitationStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        try {
            await CheckInvitationStatus(request.Token, request.UserId);
        } catch (FriendlyException e) {
            Console.WriteLine(e);
            throw;
        }

        var owner = await context.Users.FirstAsync(
            u => u.WishList!.InvitationTokens.Any(t => t.Token == request.Token), cancellationToken);

        return new InvitationStatusDto {
            Owner = owner.Name,
            PictureUrl = owner.PictureUrl,
            OwnerId = owner.Id,
        };
    }

    private async Task<InvitationToken> CheckInvitationStatus(Guid token, int userId)
    {
        var invitationToken = await context.Set<InvitationToken>()
            .Include(t => t.WishList)
            .SingleOrDefaultAsync(t => t.Token == token);
        if (invitationToken == null)
            throw new FriendlyException("Denne invitasjonen finnes ikke.");
        if (invitationToken.WishList!.UserId == userId)
            throw new FriendlyException("Du kan ikke godta en invitasjon til din egen liste.");

        return invitationToken;
    }
}
