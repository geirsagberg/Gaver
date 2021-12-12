namespace Gaver.Web.Features.Mail;

public class MailModel
{
    public IList<string> To { get; set; } = new List<string>();
    public string? From { get; set; }
    public string? Subject { get; set; }
    public string? Content { get; set; }
}