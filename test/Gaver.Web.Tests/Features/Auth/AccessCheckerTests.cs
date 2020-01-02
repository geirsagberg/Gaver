using System.Threading.Tasks;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Auth;
using Xunit;

namespace Gaver.Web.Tests.Features.Auth
{
    public class AccessCheckerTests : DbTestBase<AccessChecker>
    {
        [Fact]
        public async Task Can_access_wishList_of_other_member_in_group()
        {
            var alice = new User {
                Name = "Alice",
                WishList = new WishList()
            };
            var bob = new User {
                Name = "Bob",
                WishList = new WishList()
            };
            Context.AddRange(new UserGroup {
                Name = "Familien",
                UserGroupConnections = {
                    new UserGroupConnection {
                        User = alice
                    },
                    new UserGroupConnection {
                        User = bob
                    }
                }
            });
            await Context.SaveChangesAsync();

            await TestSubject.CheckWishListAccess(bob.WishList.Id, alice.Id);
        }
    }
}
