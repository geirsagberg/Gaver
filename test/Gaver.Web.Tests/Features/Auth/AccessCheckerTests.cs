using System.Threading.Tasks;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Auth;
using Xunit;

namespace Gaver.Web.Tests.Features.Auth;

public class AccessCheckerTests : DbTestBase<AccessChecker>
{
    [Fact]
    public async Task Can_access_wishList_of_other_member_in_group()
    {
        var alice = new User {
            Name = "Alice",
            PrimaryIdentityId = "1"
        };
        var bob = new User {
            Name = "Bob",
            PrimaryIdentityId = "2"
        };
        Context.AddRange(new UserGroup {
            Name = "Familien",
            CreatedByUser = alice,
            Users = {
                alice,
                bob
            }
        });
        await Context.SaveChangesAsync();

        await TestSubject.CheckWishListAccess(bob.WishList!.Id, alice.Id);
    }
}