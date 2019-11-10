using System.Threading;
using System.Threading.Tasks;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Models;
using MediatR;

namespace Gaver.Web.Features.MyList
{
    public class WishOptionHandler : IRequestHandler<AddWishOptionRequest, WishOptionModel>
    {
        private readonly GaverContext context;
        private readonly IMapperService mapperService;

        public WishOptionHandler(GaverContext context, IMapperService mapperService)
        {
            this.context = context;
            this.mapperService = mapperService;
        }

        public async Task<WishOptionModel> Handle(AddWishOptionRequest request, CancellationToken cancellationToken)
        {
            var wishOption = new WishOption {
                Title = request.Title,
                Url = request.Url,
                WishId = request.WishId
            };
            context.WishOptions.Add(wishOption);
            await context.SaveChangesAsync(cancellationToken);
            return mapperService.Map<WishOptionModel>(wishOption);
        }
    }
}
