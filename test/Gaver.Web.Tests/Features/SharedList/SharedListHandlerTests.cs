using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.SharedList;
using Gaver.Web.Features.SharedList.Requests;
using Xunit;

namespace Gaver.Web.Tests.Features.SharedList
{
    public class SharedListHandlerTests : DbTestBase<SharedListHandler>
    {
        [Fact]
        public async Task Can_read_shared_list()
        {
            var bob = new User {
                Name = "Bob",
                WishList = new WishList()
            };
            var james = new User {
                Name = "James"
            };
            Context.AddRange(bob, james);
            Context.SaveChanges();

            var result = await TestSubject.Handle(new GetSharedListRequest {
                WishListId = bob.WishList.Id,
                UserId = james.Id
            });

            result.OwnerUserId.Should().Be(bob.Id);
            result.Users.Should().Contain(u => u.Id == bob.Id);
        }

        [Fact]
        public async Task SharedList_includes_whether_I_have_shared_my_list_with_them()
        {
            var bob = new User {
                Name = "Bob",
                WishList = new WishList()
            };
            var james = new User {
                Name = "James",
                WishList = new WishList()
            };
            var alice = new User {
                Name = "Alice",
                WishList = new WishList {
                    Invitations = {
                        new Invitation {User = james}
                    }
                }
            };
            Context.AddRange(bob, james, alice);
            Context.SaveChanges();

            var bobResult = await TestSubject.Handle(new GetSharedListRequest {
                WishListId = james.WishList.Id,
                UserId = bob.Id
            });
            var aliceResult = await TestSubject.Handle(new GetSharedListRequest {
                WishListId = james.WishList.Id,
                UserId = alice.Id
            });

            bobResult.CanSeeMyList.Should().Be(false);
            aliceResult.CanSeeMyList.Should().Be(true);
        }
    }
}
