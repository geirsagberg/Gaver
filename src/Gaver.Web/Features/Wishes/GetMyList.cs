using System.Collections.Generic;
using System.Linq;
using Gaver.Data;
using Gaver.Data.Entities;
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

    public class GetMyListRequest : IRequest<MyListModel>
    {
        public string UserName { get; set; }
    }
}