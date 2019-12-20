using System.Threading;
using System.Threading.Tasks;
using Gaver.Data.Entities;
using Gaver.TestUtils;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Feedback;
using Gaver.Web.Features.Mail;
using NSubstitute;
using Xunit;

namespace Gaver.Web.Tests.Features.Feedback
{
    public class FeedbackHandlerTests : DbTestBase<FeedbackHandler>
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SendFeedback_should_only_include_username_if_not_anonymous(bool anonymous)
        {
            //Given
            var user = new User {
                Name = "John"
            };
            Context.Add(user);
            Context.SaveChanges();
            var request = new SubmitFeedbackRequest {
                Message = "Cool stuff",
                Anonymous = anonymous,
                UserId = user.Id
            };

            //When
            await TestSubject.Handle(request, default);

            //Then
            await Get<IMailSender>().Received(1).SendAsync(Arg.Is<MailModel>(m => anonymous ? !m.Content.Contains(user.Name) : m.Content.Contains(user.Name)), Arg.Any<CancellationToken>());
        }
    }
}
