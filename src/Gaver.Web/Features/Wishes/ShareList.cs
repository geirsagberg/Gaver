using System.Threading.Tasks;
using Flurl;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.Features.Wishes
{
    public class ShareListHandler : IAsyncRequestHandler<ShareListRequest, Unit>
    {
        private readonly IMailSender mailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShareListHandler(IMailSender mailSender, IHttpContextAccessor httpContextAccessor)
        {
            this.mailSender = mailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ShareListRequest message)
        {
            var request = _httpContextAccessor.HttpContext.Request;

            var url = Url.Combine(request.Scheme + "://" + request.Host, "list", message.WishListId.ToString());
            var mail = new Mail
            {
                To = message.Emails,
                From = "noreply@sagberg.net",
                Subject = "Noen har delt en ønskeliste med deg",
                Content = $@"<h1>Noen har delt en ønskeliste med deg!</h1>
                <p><a href='{url}'>Klikk her for å se listen.</a></p>"
            };
            await mailSender.SendAsync(mail);
            return Unit.Value;
        }
    }
}