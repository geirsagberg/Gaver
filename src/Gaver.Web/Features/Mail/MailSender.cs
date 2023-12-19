using Flurl.Http;
using Gaver.Common.Attributes;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Options;

namespace Gaver.Web.Features.Mail;

[Service]
public class MailSender(MailOptions options, IMapperService mapper, ILogger<MailSender> logger) : IMailSender {
    private readonly ILogger<MailSender> logger = logger;
    private readonly IMapperService mapper = mapper;
    private readonly MailOptions options = options;

    public async Task SendAsync(MailModel mail, System.Threading.CancellationToken cancellationToken = default) {
        if (options.SendGridApiKey.IsNullOrEmpty()) {
            throw new FriendlyException("Mangler API-nøkkel for SendGrid");
        }
        mail.From ??= "noreply@sagberg.net";
        var sendGridMail = mapper.Map<SendGridMail>(mail);
        try {
            await options.SendGridUrl
                .WithOAuthBearerToken(options.SendGridApiKey)
                .PostJsonAsync(sendGridMail);
            logger.LogInformation("Mail sent to {To}", mail.To);
        } catch (Exception e) {
            logger.LogErrorAndThrow(e, "Failed to share list");
        }
    }
}
