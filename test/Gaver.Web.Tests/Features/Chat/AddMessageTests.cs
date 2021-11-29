using System.Threading.Tasks;
using FluentAssertions;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Features.Chat;
using Gaver.Web.Features.Users;
using Xunit;

namespace Gaver.Web.Tests.Features.Chat;

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
            UserId = user.Id,
            WishListId = user.WishList!.Id
        });

        result.Should().BeEquivalentTo(new ChatMessageDto {
            Text = "Hello",
            User = new ChatUserDto {
                Id = user.Id,
                Name = user.Name
            }
        }, o => o.Excluding(m => m.Id));
    }
}
