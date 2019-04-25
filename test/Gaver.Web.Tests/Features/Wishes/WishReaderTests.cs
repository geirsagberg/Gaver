using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Wishes;
using Gaver.Web.Features.Wishes.Requests;
using LightInject;
using Xunit;

namespace Gaver.Web.Tests
{
    public class WishReaderTests : DbTestBase<WishReader>
    {
        public WishReaderTests()
        {
            context = Container.Create<GaverContext>();
        }

        private readonly GaverContext context;

        [Fact]
        public async Task Can_read_my_list()
        {
            var mapper = Get<IMapperService>();
            mapper.Profiles.Should().NotBeEmpty();
            var user = new User {
                Name = "Bob",
                WishList =
                    new WishList {
                        Title = "My list"
                    }
            };
            context.Add(user);
            context.SaveChanges();

            var result = await TestSubject.Handle(new GetMyListRequest {
                UserId = user.Id
            });

            result.Title.Should().Be("My list");
        }

        [Fact]
        public async Task Can_read_shared_list()
        {
            var bob = new User {
                Name = "Bob",
                WishList =
                    new WishList()
            };
            var james = new User {
                Name = "James"
            };
            context.AddRange(bob, james);
            context.SaveChanges();

            var result = await TestSubject.Handle(new GetSharedListRequest {
                WishListId = bob.WishList.Id,
                UserId = james.Id
            });

            result.OwnerUserId.Should().Be(bob.Id);
            result.Users.Should().Contain(u => u.Id == bob.Id);
        }
    }
}
