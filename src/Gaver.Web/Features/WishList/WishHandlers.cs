using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using MediatR;

namespace Gaver.Web.Features.WishList
{
    public class GetWishesHandler : IRequestHandler<GetWishesRequest, IList<Wish>>
    {
        private readonly GaverContext context;

        public GetWishesHandler(GaverContext context)
        {
            this.context = context;
        }

        public IList<Wish> Handle(GetWishesRequest message)
        {
            return context.Set<Wish>().ToList();
        }
    }

    public class ShareListHandler : IAsyncRequestHandler<ShareListRequest>
    {
        private readonly IMailSender mailSender;

        public ShareListHandler(IMailSender mailSender)
        {
            this.mailSender = mailSender;
        }

        public async Task Handle(ShareListRequest message)
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