using System.Linq;
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
    }
}
