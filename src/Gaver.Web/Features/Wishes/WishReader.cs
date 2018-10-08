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
using Gaver.Web.Constants;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Wishes.Models;
using Gaver.Web.Features.Wishes.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Wishes
{
    public class WishReader : IRequestHandler<GetMyListRequest, MyListModel>,
        IRequestHandler<GetSharedListRequest, SharedListModel>,
        IRequestHandler<CheckSharedListAccessRequest, ListAccessStatus>
    {
        private readonly IAccessChecker accessChecker;
        private readonly GaverContext context;
        private readonly IMapperService mapper;

        public WishReader(GaverContext context, IMapperService mapper, IAccessChecker accessChecker)
        {
            this.context = context;
            this.mapper = mapper;
            this.accessChecker = accessChecker;
        }

        public async Task<ListAccessStatus> Handle(CheckSharedListAccessRequest request,
            CancellationToken token = default)
        {
            var wishListOwnerId = await context.WishLists.Where(wl => wl.Id == request.WishListId)
                .Select(wl => wl.UserId)
                .SingleOrDefaultAsync(token);
            if (wishListOwnerId == 0)
                throw new EntityNotFoundException<WishList>(request.WishListId);

            if (wishListOwnerId == request.UserId)
                return ListAccessStatus.Owner;

            if (await context.Invitations.AnyAsync(
                wl => wl.WishListId == request.WishListId && wl.UserId == request.UserId, token))
                return ListAccessStatus.Invited;

            return ListAccessStatus.NotInvited;
        }

        public async Task<MyListModel> Handle(GetMyListRequest message, CancellationToken token = default)
        {
            var model = GetModel(message);
            if (model == null) {
                context.Add(new WishList {
                    UserId = message.UserId
                });
                await context.SaveChangesAsync(token);
            }

            return GetModel(message);
        }

        public Task<SharedListModel> Handle(GetSharedListRequest message, CancellationToken cancellationToken = default)
        {
            if (context.Set<WishList>().Any(wl => wl.Id == message.ListId && wl.UserId == message.UserId)) {
                throw new FriendlyException(EventIds.OwnerAccessingSharedList, "Du kan ikke se din egen liste");
            }

            accessChecker.CheckWishListInvitations(message.ListId, message.UserId);

            var sharedListModel = context.Set<WishList>()
                .Where(wl => wl.Id == message.ListId)
                .ProjectTo<SharedListModel>(mapper.MapperConfiguration)
                .SingleOrThrow(new FriendlyException(EventIds.SharedListMissing, "Listen finnes ikke"));
            return Task.FromResult(sharedListModel);
        }

        private MyListModel GetModel(GetMyListRequest message)
        {
            var model = context.Set<WishList>()
                .Where(wl => wl.UserId == message.UserId)
                .ProjectTo<MyListModel>(mapper.MapperConfiguration)
                .Single();
            model.Invitations = context.Invitations.Where(i => i.UserId == message.UserId)
                .ProjectTo<InvitationModel>(mapper.MapperConfiguration)
                .ToList();
            return model;
        }
    }
}