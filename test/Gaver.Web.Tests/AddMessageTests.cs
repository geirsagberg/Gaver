using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Chat;
using Gaver.Web.Features.Users;
using Xunit;

namespace Gaver.Web.Tests
{
    public class AddMessageTests : DbTestBase<AddMessageHandler>
    {
        [Fact]
        public async Task Can_add_chatMessage()
        {
            var user = new User {
                Name = "Userman",
                PrimaryIdentityId = "1"
            };
            Context.Add(user);
            Context.SaveChanges();

            var result = await TestSubject.Handle(new AddMessageRequest {
                Text = "Hello",
                UserId = user.Id
            });

            result.Should().BeEquivalentTo(new ChatMessageModel {
                Text = "Hello",
                User = new UserModel {
                    Id = user.Id,
                    Name = user.Name
                }
            }, o => o.Excluding(m => m.Id));
        }
    }
}
