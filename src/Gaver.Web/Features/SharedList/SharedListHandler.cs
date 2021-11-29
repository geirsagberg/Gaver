using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Common.Extensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Data.Exceptions;
using Gaver.Web.Contracts;
using Gaver.Web.Features.SharedList.Requests;
using Gaver.Web.Features.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.SharedList;

public class SharedListHandler :
    IRequestHandler<SetBoughtRequest, SharedWishDto>,
    IRequestHandler<GetSharedListRequest, SharedListDto>,
    IRequestHandler<CheckSharedListAccessRequest, ListAccessStatus>
{
    private readonly IClientNotifier clientNotifier;
    private readonly GaverContext context;
    private readonly IMapperService mapper;

    public SharedListHandler(GaverContext context, IMapperService mapper, IClientNotifier clientNotifier)
    {
        this.context = context;
        this.mapper = mapper;
        this.clientNotifier = clientNotifier;
    }

    public async Task<ListAccessStatus> Handle(CheckSharedListAccessRequest request,
        CancellationToken cancellationToken = default)
    {
        var wishListOwnerId = await context.WishLists.Where(wl => wl.Id == request.WishListId)
            .Select(wl => wl.UserId)
            .SingleOrDefaultAsync(cancellationToken);
        if (wishListOwnerId == 0)
            throw new EntityNotFoundException<WishList>(request.WishListId);

        if (wishListOwnerId == request.UserId)
            return ListAccessStatus.Owner;

        if (await context.WishLists.AnyAsync(
                wl => wl.Id == request.WishListId && wl.User!.Friends.Any(f => f.Id == request.UserId), cancellationToken))
            return ListAccessStatus.Invited;

        return ListAccessStatus.NotInvited;
    }

    public async Task<SharedListDto> Handle(GetSharedListRequest message,
        CancellationToken cancellationToken = default)
    {
        var results = await context.Set<WishList>()
            .Where(wl => wl.Id == message.WishListId)
            .ProjectTo<SharedListDto>(mapper.MapperConfiguration)
            .ToListAsync(cancellationToken);

        var model = results.SingleOrThrow(new FriendlyException("Listen finnes ikke"));

        var owner = await context.Set<User>()
            .Where(u => u.WishList!.Id == message.WishListId)
            .ProjectTo<UserDto>(mapper.MapperConfiguration)
            .SingleAsync(cancellationToken);

        // var canSeeMyList = await context.Set<Invitation>()
        //     .AnyAsync(i => i.WishList!.UserId == message.UserId && i.UserId == owner.Id, cancellationToken);

        model.CanSeeMyList = true;

        model.Users.Add(owner);
        return model;
    }

    public async Task<SharedWishDto> Handle(SetBoughtRequest message, CancellationToken cancellationToken)
    {
        var wish = GetWish(message.WishId, message.WishListId);
        var userId = message.UserId;
        if (wish.BoughtByUserId != null && wish.BoughtByUserId != userId)
            throw new FriendlyException("Wish has already been bought by someone else");

        if (message.IsBought) wish.BoughtByUserId = userId;
        else wish.BoughtByUserId = null;
        await context.SaveChangesAsync(cancellationToken);

        await clientNotifier.RefreshListAsync(message.WishListId, userId);
        return mapper.Map<SharedWishDto>(wish);
    }

    private Wish GetWish(int wishId, int wishListId)
    {
        var wish = context.GetOrDie<Wish>(wishId);
        if (wish.WishListId != wishListId)
            throw new FriendlyException($"Wish {wishId} does not belong to list {wishListId}");
        return wish;
    }
}