using System.Linq;
using AutoMapper.QueryableExtensions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Logic.Contracts;
using MediatR;

namespace Gaver.Web.Features.Wishes
{
    public class GetSharedListRequest : IRequest<SharedListModel>
    {
        public int ListId { get; set; }
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
            return context.Set<WishList>()
                .Where(wl => wl.Id == message.ListId)
                .ProjectTo<SharedListModel>(mapperService.MapperConfiguration)
                .Single();
        }
    }
}