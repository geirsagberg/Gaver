using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Gaver.Common.Exceptions;
using Gaver.Common.Extensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Mail;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.Features.MyList
{
    public class WishMailer : IRequestHandler<ShareListRequest>
    {
        private readonly IMailSender mailSender;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly GaverContext gaverContext;

        public WishMailer(IMailSender mailSender, IHttpContextAccessor httpContextAccessor, GaverContext gaverContext)
        {
            this.mailSender = mailSender;
            this.httpContextAccessor = httpContextAccessor;
            this.gaverContext = gaverContext;
        }

        public async Task<Unit> Handle(ShareListRequest message, CancellationToken cancellationToken)
        {
            ValidateEmails(message.Emails);
            var userName = gaverContext.Users.Where(u => u.Id == message.UserId).Select(u => u.Name).Single();
            var wishListId = gaverContext.WishLists.Where(wl => wl.UserId == message.UserId).Select(wl => wl.Id).Single();
            var request = httpContextAccessor.HttpContext.Request;

            var mailTasks = new List<Task>();
            foreach (var email in message.Emails) {
                var token = new InvitationToken {
                    WishListId = wishListId
                };
                gaverContext.Set<InvitationToken>().Add(token);

                var url = Url.Combine(request.Scheme + "://" + request.Host, "invitations", token.Token.ToString());
                var mail = new MailModel {
                    To = new[] { email },
                    From = "noreply@sagberg.net",
                    Subject = $"{userName} har delt en ønskeliste med deg",
                    Content = $@"<h1>{userName} har delt en ønskeliste med deg!</h1>
                <p><a href='{url}'>Klikk her for å se listen.</a></p>"
                };
                mailTasks.Add(mailSender.SendAsync(mail));
            }

            await gaverContext.SaveChangesAsync(cancellationToken);

            await Task.WhenAll(mailTasks);
            return Unit.Value;
        }

        private static void ValidateEmails(IEnumerable<string> emails)
        {
            var invalidEmails = emails.Where(e => !e.IsValidEmail()).ToList();
            if (invalidEmails.Any()) {
                throw new FriendlyException("Ugyldig e-postformat: " + invalidEmails.ToJoinedString());
            }
        }
    }
}
