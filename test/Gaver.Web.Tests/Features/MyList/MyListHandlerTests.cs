using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Common.Contracts;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.MyList;
using Gaver.Web.Features.Wishes.Requests;
using Xunit;

namespace Gaver.Web.Tests.Features.MyList
{
    public class MyListHandlerTests : DbTestBase<MyListHandler>
    {
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
            Context.Add(user);
            Context.SaveChanges();

            var result = await TestSubject.Handle(new GetMyListRequest {
                UserId = user.Id
            });

            result.Title.Should().Be("My list");
        }
    }
}
