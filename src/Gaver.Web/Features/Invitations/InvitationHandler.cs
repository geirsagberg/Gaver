using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Invitations
{
    public class InvitationHandler : IRequestHandler<GetInvitationStatusRequest, InvitationStatusModel>,
        IRequestHandler<AcceptInvitationRequest, InvitationModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapperService;

        public InvitationHandler(GaverContext context, IMapperService mapperService)
        {
            this.context = context;
            this.mapperService = mapperService;
        }

        public async Task<InvitationModel> Handle(AcceptInvitationRequest request,
            CancellationToken cancellationToken = default)
        {
            var invitationToken = await CheckInvitationStatus(request.Token, request.UserId);
            var invitation = new Invitation {
                UserId = request.UserId,
                WishListId = invitationToken.WishListId
            };
            context.Add(invitation);
            invitationToken.Accepted = DateTimeOffset.Now;
            await context.SaveChangesAsync(cancellationToken);
            await context.Entry(invitation).Reference(i => i.WishList).LoadAsync(cancellationToken);
            return mapperService.Map<InvitationModel>(invitation);
        }

        public async Task<InvitationStatusModel> Handle(GetInvitationStatusRequest request,
            CancellationToken cancellationToken = default)
        {
            try {
                await CheckInvitationStatus(request.Token, request.UserId);
            } catch (FriendlyException e) {
                Console.WriteLine(e);
                throw;
            }

            var owner = await context.Users.FirstAsync(
                u => u.WishList.InvitationTokens.Any(t => t.Token == request.Token), cancellationToken);

            return new InvitationStatusModel {
                Ok = true,
                Owner = owner.Name,
                PictureUrl = owner.PictureUrl
            };
        }

        private async Task<InvitationToken> CheckInvitationStatus(Guid token, int userId)
        {
            var invitation = await context.Set<InvitationToken>()
                .Include(t => t.WishList)
                .SingleOrDefaultAsync(t => t.Token == token);
            if (invitation == null)
                throw new FriendlyException("Denne invitasjonen finnes ikke.");
            if (invitation.Accepted.HasValue)
                throw new FriendlyException("Denne invitasjonen er allerede brukt.");
            if (invitation.WishList.UserId == userId)
                throw new FriendlyException("Du kan ikke godta en invitasjon til din egen liste.");

            if (await context.Set<Invitation>()
                .AnyAsync(i => i.UserId == userId && i.WishListId == invitation.WishListId))
                throw new FriendlyException("Du har allerede tilgang til denne listen.");

            return invitation;
        }
    }
}
