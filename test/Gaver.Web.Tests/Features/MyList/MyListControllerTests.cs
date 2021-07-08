using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.Web.MvcUtils;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.MyList
{
    public class MyListControllerTests : WebTestBase
    {
        public MyListControllerTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper) : base(webAppFactory, testOutputHelper)
        {
        }

        [Fact]
        public async Task Can_delete_wish()
        {
            var wish = new Wish {
                Title = "My wish"
            };
            var user = new User {
                Name = "Bob",
                WishList = new WishList {
                    Title = "My list",
                    Wishes = {
                        wish
                    }
                }
            };
            GaverContext.Users.Add(user);
            GaverContext.SaveChanges();
            RoleConfig.AddClaim(GaverClaimTypes.GaverUserId, user.Id.ToString());

            var response = await Client.DeleteAsync($"/api/MyList/{wish.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
