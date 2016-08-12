using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaver.Logic;
using Gaver.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.WishList
{
    public class ShareListHandler : AsyncRequestHandler<ShareListRequest>
    {
        private readonly IMailSender mailSender;

        public ShareListHandler(IMailSender mailSender)
        {
            this.mailSender = mailSender;
        }

        protected override async Task HandleCore(ShareListRequest message)
        {
            var mail = new Mail
            {
                To = message.Emails,
                From = "noreply@sagberg.net",
                Subject = "Noen har delt en ønskeliste med deg",
                Content = @"<h1>Noen har delt en ønskeliste med deg!</h1>
                <p><a href='http://localhost/5000'>Klikk her for å se listen.</a></p>"
            };
            await mailSender.SendAsync(mail);
        }
    }
}
