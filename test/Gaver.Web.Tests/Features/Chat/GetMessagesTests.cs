using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Chat;
using Xunit;

namespace Gaver.Web.Tests.Features.Chat
{
    public class GetMessagesTests : DbTestBase<GetMessagesHandler>
    {
        [Fact]
        public async Task Can_get_chatMessages()
        {
            var firstWishList = new WishList {
                User = new User(),
                ChatMessages = {
                    new ChatMessage {
                        Text = "Hello",
                        User = new User {
                            WishList = new WishList()
                        }
                    }
                }
            };
            var secondWishList = new WishList {
                User = new User(),
                ChatMessages = {
                    new ChatMessage {
                        User = new User {WishList = new WishList()},
                        Text = "Whatup"
                    }
                }
            };
            Context.AddRange(firstWishList, secondWishList);
            Context.SaveChanges();

            var result = await TestSubject.Handle(new GetMessagesRequest {
                WishListId = firstWishList.Id
            });

            result.Messages.Select(m => m.Text).Should().BeEquivalentTo("Hello");
        }
    }
}
