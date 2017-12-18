using AutoMapper.QueryableExtensions;
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
        public void Can_add_chatMessage()
        {
            var user = new User {
                Name = "Userman"
            };
            Context.Add(user);
            Context.SaveChanges();

            var result = TestSubject.Handle(new AddMessageRequest {
                Text = "Hello",
                UserId = user.Id
            });

            result.ShouldBeEquivalentTo(new ChatMessageModel {
                Text = "Hello",
                User = new UserModel {
                    Id = user.Id,
                    Name = user.Name
                }
            }, o => o.Excluding(m => m.Id));
        }
    }
}