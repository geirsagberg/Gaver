using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Features.Shared.Models;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class WishOptionHandler(GaverContext context, IMapperService mapperService) : IRequestHandler<AddWishOptionRequest, WishOptionDto> {
    private readonly GaverContext context = context;
    private readonly IMapperService mapperService = mapperService;

    public async Task<WishOptionDto> Handle(AddWishOptionRequest request, CancellationToken cancellationToken) {
        var wishOption = new WishOption {
            Title = request.Title,
            Url = request.Url,
            WishId = request.WishId
        };
        context.WishOptions.Add(wishOption);
        await context.SaveChangesAsync(cancellationToken);
        return mapperService.Map<WishOptionDto>(wishOption);
    }
}
