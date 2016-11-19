using System.Threading.Tasks;

namespace Gaver.Logic.Contracts
{
    public interface IMailSender
    {
        Task SendAsync(MailModel mail);
    }
}