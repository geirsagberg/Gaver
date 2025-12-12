using Flurl;
using Gaver.Data;
using Gaver.Web.Features.Utils;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class WishMailer(IHostUrlAccessor hostUrlAccessor, GaverContext gaverContext) : IRequestHandler<ShareListRequest, ShareListResponse> {
    public async Task<ShareListResponse> Handle(ShareListRequest message, CancellationToken cancellationToken) {
        var wishListId = gaverContext.WishLists.Where(wl => wl.UserId == message.UserId).Select(wl => wl.Id).Single();
        
        var shareUrl = Url.Combine(hostUrlAccessor.GetHostUrl(), "list", wishListId.ToString());
        
        return await Task.FromResult(new ShareListResponse {
            ShareUrl = shareUrl
        });
    }
}
