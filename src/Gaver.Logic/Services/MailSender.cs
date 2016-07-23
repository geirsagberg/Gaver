using System;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace Gaver.Logic
{
    public class MailSender : IMailSender
    {
        private readonly MailOptions options;
        private readonly IMapperService mapper;

        public MailSender(IOptions<MailOptions> options, IMapperService mapper) {
            this.options = options.Value;
            this.mapper = mapper;
        }

        public async Task SendAsync(Mail mail)
        {
            var sendGridMail = mapper.Map<Mail, SendGridMail>(mail);
            var response = await options.SendGridUrl
                .WithOAuthBearerToken(options.SendGridApiKey)
                .PostJsonAsync(sendGridMail);
        }
    }
}