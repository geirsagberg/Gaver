using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using Gaver.Web.Features.Wishes.Requests;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.Features.Wishes
{
    public class WishMailer : IAsyncRequestHandler<ShareListRequest>
    {
        private readonly IMailSender mailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GaverContext _gaverContext;

        public WishMailer(IMailSender mailSender, IHttpContextAccessor httpContextAccessor, GaverContext gaverContext)
        {
            this.mailSender = mailSender;
            _httpContextAccessor = httpContextAccessor;
            _gaverContext = gaverContext;
        }

        public async Task HandleAsync(ShareListRequest message)
        {
            var userName = _gaverContext.Set<User>().Where(u => u.Id == message.UserId).Select(u => u.Name).Single();
            var request = _httpContextAccessor.HttpContext.Request;

            var url = Url.Combine(request.Scheme + "://" + request.Host, "list", message.WishListId.ToString());
            var mail = new Mail
            {
                To = message.Emails,
                From = "noreply@sagberg.net",
                Subject = $"{userName} har delt en ønskeliste med deg",
                Content = $@"<h1>{userName} har delt en ønskeliste med deg!</h1>
                <p><a href='{url}'>Klikk her for å se listen.</a></p>"
            };
            await mailSender.SendAsync(mail);
        }
    }
}