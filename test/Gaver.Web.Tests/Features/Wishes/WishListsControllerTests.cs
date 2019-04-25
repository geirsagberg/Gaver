using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Features.Wishes.Requests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Gaver.Web.Tests.Features.Wishes
{
    public class WishListsControllerTests : WebTestBase
    {
        public WishListsControllerTests(CustomWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
        }

        [Fact]
        public async Task Cannot_update_wish_from_another_list()
        {
            var wish = new Wish {
                Title = "Sjokolade"
            };
            var wishList = new WishList {
                Wishes = {
                    wish
                }
            };
            var gaverContext = ServiceProvider.GetRequiredService<GaverContext>();
            gaverContext.Add(new User {
                Name = "TestUser",
                Email = "user@example.com",
                PrimaryIdentityId = "abc",
                WishList = wishList
            });
            gaverContext.SaveChanges();
            // TODO: Set fake authentication or real one

            var response = await Client.PostAsJsonAsync($"/api/WishLists/{wishList.Id}/{wish.Id}/Title",
                new SetTitleRequest {Title = "Abc"});

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
