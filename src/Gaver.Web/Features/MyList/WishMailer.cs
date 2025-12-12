using Flurl;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Features.Utils;
using MediatR;

namespace Gaver.Web.Features.MyList;

public class WishMailer(IHostUrlAccessor hostUrlAccessor, GaverContext gaverContext) : IRequestHandler<ShareListRequest, ShareListResponse> {
    public async Task<ShareListResponse> Handle(ShareListRequest message, CancellationToken cancellationToken) {
        var wishListId = gaverContext.WishLists.Where(wl => wl.UserId == message.UserId).Select(wl => wl.Id).Single();
        
        // Create a new invitation token that can be used by multiple people
        var token = new InvitationToken {
            WishListId = wishListId
        };
        gaverContext.Set<InvitationToken>().Add(token);
        await gaverContext.SaveChangesAsync(cancellationToken);
        
        var shareUrl = Url.Combine(hostUrlAccessor.GetHostUrl(), "invitations", token.Token.ToString());
        
        return new ShareListResponse {
            ShareUrl = shareUrl
        };
    }
}
