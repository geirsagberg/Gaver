using Flurl.Http;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Common.Extensions;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Options;

namespace Gaver.Web.Features.Mail;

// [Service]
public class SendGridMailSender(MailOptions options, IMapperService mapper, ILogger<SendGridMailSender> logger) : IMailSender {
    public async Task SendAsync(MailModel mail, CancellationToken cancellationToken = default) {
        if (options.SendGridApiKey.IsNullOrEmpty()) {
            throw new FriendlyException("Mangler API-nøkkel for SendGrid");
        }

        mail.From ??= "noreply@sagberg.net";
        var sendGridMail = mapper.Map<SendGridMail>(mail);
        try {
            await options.SendGridUrl
                .WithOAuthBearerToken(options.SendGridApiKey)
                .PostJsonAsync(sendGridMail, HttpCompletionOption.ResponseContentRead, cancellationToken);
            logger.LogInformation("Mail sent to {To}", mail.To);
        } catch (Exception e) {
            logger.LogErrorAndThrow(e, "Failed to share list");
        }
    }
}
