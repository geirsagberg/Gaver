using System.Linq;
using System.Threading.Tasks;
using Gaver.Data;
using Gaver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Web.Extensions
{
    public static class GaverContextExtensions
    {
        //    public static async Task<int> GetUserIdOrDie(this GaverContext gaverContext, ClaimsPrincipal principal)
        //    {
        //        var primaryIdentityId = principal.GetPrimaryIdentityId();
        //        var userId = await gaverContext.Set<User>().Where(u => u.PrimaryIdentityId == primaryIdentityId)
        //            .Select(u => u.Id)
        //            .SingleOrDefaultAsync();
        //        if (userId == default) {
        //            throw new FriendlyException(EventIds.UnknownUserId, "Brukeren finnes ikke");
        //        }

        //        return userId;
        //    }

        //    public static async Task<User> GetUserOrDie(this GaverContext gaverContext, ClaimsPrincipal principal)
        //    {
        //        var primaryIdentityId = principal.GetPrimaryIdentityId();
        //        var user = await gaverContext.Set<User>()
        //            .SingleOrDefaultAsync(u => u.PrimaryIdentityId == primaryIdentityId);
        //        if (user == null) {
        //            throw new FriendlyException(EventIds.UnknownUserId, "Brukeren finnes ikke");
        //        }

        //        return user;
        //    }
        public static async Task<int> GetUserWishListId(this GaverContext context, int userId)
        {
            return await context.Set<WishList>().Where(wl => wl.UserId == userId).Select(wl => wl.Id).SingleAsync();
        }
    }
}
