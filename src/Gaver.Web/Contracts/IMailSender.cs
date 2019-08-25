using System.Threading.Tasks;
using Gaver.Web.Features.Mail;

namespace Gaver.Web.Contracts
{
    public interface IMailSender
    {
        Task SendAsync(MailModel mail, System.Threading.CancellationToken cancellationToken = default);
    }
}
