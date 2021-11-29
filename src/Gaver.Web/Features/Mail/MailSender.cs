using System;
using System.Threading.Tasks;
using Flurl.Http;
using Gaver.Common.Attributes;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Options;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Features.Mail;

[Service]
public class MailSender : IMailSender
{
    private readonly ILogger<MailSender> logger;
    private readonly IMapperService mapper;
    private readonly MailOptions options;

    public MailSender(MailOptions options, IMapperService mapper, ILogger<MailSender> logger)
    {
        this.options = options;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task SendAsync(MailModel mail, System.Threading.CancellationToken cancellationToken = default)
    {
        if (options.SendGridApiKey.IsNullOrEmpty()) {
            throw new FriendlyException("Mangler API-nøkkel for SendGrid");
        }
        mail.From ??= "noreply@sagberg.net";
        var sendGridMail = mapper.Map<SendGridMail>(mail);
        try {
            await options.SendGridUrl
                .WithOAuthBearerToken(options.SendGridApiKey)
                .PostJsonAsync(sendGridMail, cancellationToken);
            logger.LogInformation("Mail sent to {To}", mail.To);
        } catch (Exception e) {
            logger.LogErrorAndThrow(e, "Failed to share list");
        }
    }
}