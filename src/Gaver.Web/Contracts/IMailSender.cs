using Gaver.Web.Features.Mail;

namespace Gaver.Web.Contracts;

public interface IMailSender
{
    Task SendAsync(MailModel mail, CancellationToken cancellationToken = default);
}