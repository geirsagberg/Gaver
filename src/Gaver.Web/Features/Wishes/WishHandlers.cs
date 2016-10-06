using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Execution;
using AutoMapper.QueryableExtensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Features.Wishes
{
    public class GetMyListHandler : IRequestHandler<GetMyListRequest, MyListModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapperService;

        public GetMyListHandler(GaverContext context, IMapperService mapperService)
        {
            this.context = context;
            this.mapperService = mapperService;
        }

        public MyListModel Handle(GetMyListRequest message)
        {
            var wishList = context.Set<WishList>().Include(wl => wl.Wishes).Single(wl => wl.User.Name == message.UserName);
            var wishModels = mapperService.Map<IList<WishModel>>(wishList.Wishes);
            return new MyListModel
            {
                Id = wishList.Id,
                Title = wishList.Title,
                Wishes = wishModels
            };
        }
    }

    public class GetSharedListHandler : IRequestHandler<GetSharedListRequest, SharedListModel> {
        private readonly GaverContext context;
        private readonly IMapperService mapperService;

        public GetSharedListHandler(GaverContext context, IMapperService mapperService)
        {
            this.context = context;
            this.mapperService = mapperService;
        }

        public SharedListModel Handle(GetSharedListRequest message)
        {
            var wishList = context.Set<WishList>().Where(wl => wl.Id == message.ListId)
                .ProjectTo<SharedListModel>(mapperService.MapperConfiguration).Single();

            return wishList;
        }
    }

    public class AddWishHandler : IRequestHandler<AuthenticatedAddWishRequest, WishModel>
    {

        private readonly GaverContext context;
        private readonly IMapperService mapperService;

        public AddWishHandler(GaverContext context, IMapperService mapperService)
        {
            this.context = context;
            this.mapperService = mapperService;
        }

        public WishModel Handle(AuthenticatedAddWishRequest message)
        {
            var wishListId = context.Users.Where(u => u.Name == message.UserName).Select(u => u.WishLists.Single().Id).Single();
            var wish = new Wish
            {
                Title = message.Title,
                WishListId = wishListId
            };
            context.Add(wish);
            context.SaveChanges();
            return mapperService.Map<WishModel>(wish);
        }
    }

    public class DeleteWishHandler : IRequestHandler<DeleteWishRequest, Unit>
    {

        private readonly GaverContext context;

        public DeleteWishHandler(GaverContext context)
        {
            this.context = context;
        }

        public Unit Handle(DeleteWishRequest message)
        {
            context.Delete<Wish>(message.WishId);
            context.SaveChanges();
            return Unit.Value;
        }
    }

    public class ShareListHandler : IAsyncRequestHandler<ShareListRequest, Unit>
    {
        private readonly IMailSender mailSender;

        public ShareListHandler(IMailSender mailSender)
        {
            this.mailSender = mailSender;
        }

        public async Task<Unit> Handle(ShareListRequest message)
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
            return Unit.Value;
        }
    }
}