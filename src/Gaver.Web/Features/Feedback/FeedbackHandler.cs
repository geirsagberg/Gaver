using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.Contracts;
using Gaver.Web.Features.Mail;
using Gaver.Web.Options;
using MediatR;

namespace Gaver.Web.Features.Feedback;

public class FeedbackHandler(IMailSender mailSender, MailOptions mailOptions, GaverContext context) : IRequestHandler<SubmitFeedbackRequest> {
    public async Task Handle(SubmitFeedbackRequest request, CancellationToken cancellationToken) {
        var content = request.Message;

        if (!request.Anonymous) {
            var user = await context.GetOrDieAsync<User>(request.UserId);

            content += "<br><br>Vennlig hilsen " + user.Name;
        }

        var mail = new MailModel {
            To = { mailOptions.FeedbackAddress },
            Subject = "Gaver - Feedback",
            Content = content
        };
        await mailSender.SendAsync(mail, cancellationToken);


    }
}
