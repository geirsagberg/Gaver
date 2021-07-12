using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.Web.Features.MyList;
using Gaver.Web.MvcUtils;
using Gaver.Web.Tests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.MyList
{
    public class MyListControllerTests : WebTestBase
    {
        private readonly Wish wish;

        public MyListControllerTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper) : base(webAppFactory, testOutputHelper)
        {
            wish = new Wish {
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
            RoleConfig.Reset();
            RoleConfig.AddClaim(GaverClaimTypes.GaverUserId, user.Id.ToString());
        }

        [Fact]
        public async Task Can_delete_wish()
        {
            var response = await Client.DeleteAsync($"/api/myList/{wish.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Can_update_wish()
        {
            var response = await Client.PatchAsJsonAsync($"/api/myList/{wish.Id}", new UpdateWishRequest {
                Url = "google.com"
            });

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
