using System;
using System.Threading.Tasks;
using Flurl.Http;
using Gaver.Common.Contracts;
using Gaver.Common.Exceptions;
using Gaver.Data;
using Gaver.Web.Constants;
using Gaver.Web.Contracts;
using Gaver.Web.Extensions;
using Gaver.Web.Options;
using Microsoft.Extensions.Logging;

namespace Gaver.Web.Features.Mail
{
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

        public async Task SendAsync(MailModel mail)
        {
            if (options.SendGridApiKey.IsNullOrEmpty()) {
                throw new FriendlyException(EventIds.SendGridApiKeyMissing, "Mangler API-nøkkel for SendGrid");
            }
            var sendGridMail = mapper.Map<SendGridMail>(mail);
            try {
                await options.SendGridUrl
                    .WithOAuthBearerToken(options.SendGridApiKey)
                    .PostJsonAsync(sendGridMail);
                logger.LogInformation("Mail sent to {To}", mail.To);
            } catch (Exception e) {
                logger.LogErrorAndThrow(EventIds.ShareListFailed, e, "Failed to share list");
            }
        }
    }
}