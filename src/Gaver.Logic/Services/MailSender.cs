using System;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace Gaver.Logic
{
    public class MailSender : IMailSender
    {
        private readonly MailOptions options;

        public MailSender(IOptions<MailOptions> options) {
            this.options = options.Value;
        }

        public void Send(Mail mail)
        {

        }
    }
}