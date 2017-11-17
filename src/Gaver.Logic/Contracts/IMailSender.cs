using System.Threading.Tasks;
using Gaver.Logic.Features.Mail;

namespace Gaver.Logic.Contracts
{
    public interface IMailSender
    {
        Task SendAsync(MailModel mail);
    }
}