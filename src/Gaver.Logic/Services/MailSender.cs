using System;
using System.Threading.Tasks;
using Flurl.Http;
using Gaver.Logic.Constants;
using Gaver.Logic.Contracts;
using Gaver.Data;
using Gaver.Logic.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gaver.Logic.Services
{
    public class MailSender : IMailSender
    {
        private readonly MailOptions options;
        private readonly IMapperService mapper;
        private readonly ILogger<MailSender> logger;

        public MailSender(IOptions<MailOptions> options, IMapperService mapper, ILogger<MailSender> logger)
        {
            this.options = options.Value;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task SendAsync(MailModel mail)
        {
            if (options.SendGridApiKey.IsNullOrEmpty())
                throw new FriendlyException(EventIds.SendGridApiKeyMissing, "Mangler API-nøkkel for SendGrid");
            var sendGridMail = mapper.Map<SendGridMail>(mail);
            try
            {
                await options.SendGridUrl
                    .WithOAuthBearerToken(options.SendGridApiKey)
                    .PostJsonAsync(sendGridMail);
                logger.LogInformation("Mail sent to {To}", mail.To);
            }
            catch (Exception e)
            {
                logger.LogErrorAndThrow(EventIds.ShareListFailed, e, "Failed to share list");
            }
        }
    }
}