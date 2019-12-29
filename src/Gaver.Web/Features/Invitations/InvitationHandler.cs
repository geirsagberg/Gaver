using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Invitations
{
    public class InvitationHandler : IRequestHandler<GetInvitationStatusRequest, InvitationStatusDto>,
        IRequestHandler<AcceptInvitationRequest, FriendDto>
    {
        private readonly GaverContext context;

        public InvitationHandler(GaverContext context)
        {
            this.context = context;
        }

        public async Task<FriendDto> Handle(AcceptInvitationRequest request,
            CancellationToken cancellationToken = default)
        {
            var invitationToken = await CheckInvitationStatus(request.Token, request.UserId);

            var invitation = new Invitation {
                UserId = request.UserId,
                WishListId = invitationToken.WishListId
            };
            context.Add(invitation);

            var wishList = await context.WishLists.Include(wl => wl.User)
                .SingleAsync(wl => wl.Id == invitationToken.WishListId, cancellationToken);
            var user = await context.Users.Include(u => u.WishList)
                .SingleAsync(u => u.Id == request.UserId, cancellationToken);
            if (!await context.Invitations.AnyAsync(
                i => i.WishListId == user.WishList!.Id && i.UserId == wishList.UserId,
                cancellationToken)) {
                var otherInvitation = new Invitation {
                    UserId = wishList.UserId,
                    WishListId = user.WishList!.Id
                };
                context.Add(otherInvitation);
            }

            context.AddRange(
                new UserFriendConnection {
                    UserId = request.UserId,
                    FriendId = wishList.UserId
                },
                new UserFriendConnection {
                    UserId = wishList.UserId,
                    FriendId = request.UserId
                }
            );

            invitationToken.Accepted = DateTimeOffset.Now;

            await context.SaveChangesAsync(cancellationToken);
            var userName = await context.Set<User>().Where(u => u.WishList!.Id == invitationToken.WishListId)
                .Select(u => u.Name).SingleAsync(cancellationToken);
            return new FriendDto {
                WishListId = invitationToken.WishListId,
                UserName = userName
            };
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
                Ok = true,
                Owner = owner.Name,
                PictureUrl = owner.PictureUrl
            };
        }

        private async Task<InvitationToken> CheckInvitationStatus(Guid token, int userId)
        {
            var invitationToken = await context.Set<InvitationToken>()
                .Include(t => t.WishList)
                .SingleOrDefaultAsync(t => t.Token == token);
            if (invitationToken == null)
                throw new FriendlyException("Denne invitasjonen finnes ikke.");
            if (invitationToken.Accepted.HasValue)
                throw new FriendlyException("Denne invitasjonen er allerede brukt.");
            if (invitationToken.WishList!.UserId == userId)
                throw new FriendlyException("Du kan ikke godta en invitasjon til din egen liste.");

            if (await context.Set<Invitation>()
                .AnyAsync(i => i.UserId == userId && i.WishListId == invitationToken.WishListId))
                throw new FriendlyException("Du har allerede tilgang til denne listen.");

            return invitationToken;
        }
    }
}
