using System.Net;
using System.Net.Mail;
using System.Text;
using Gaver.Common.Attributes;
using Gaver.Common.Extensions;
using Gaver.Web.Contracts;
using Gaver.Web.Options;

namespace Gaver.Web.Features.Mail;

[Service]
public class SmtpMailSender(MailOptions options) : IMailSender {
    private readonly SmtpClient smtpClient = new(options.SmtpServer, options.SmtpPort) {
        Credentials = new NetworkCredential(options.SmtpUsername, options.SmtpPassword),
        EnableSsl = true
    };

    public async Task SendAsync(MailModel mail, CancellationToken cancellationToken = default) {
        var mailMessage = new MailMessage(
            options.NoReplyAddress,
            mail.To.ToJoinedString(),
            mail.Subject,
            mail.Content
        );
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.IsBodyHtml = true;
        mailMessage.From = new MailAddress(options.NoReplyAddress, "Gaver");

        await smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }
}
