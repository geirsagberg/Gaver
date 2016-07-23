using System.Threading.Tasks;

namespace Gaver.Logic
{
  public interface IMailSender {
    Task SendAsync(Mail mail);
  }
}