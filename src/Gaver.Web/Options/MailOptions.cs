namespace Gaver.Web.Options;

public class MailOptions {
    public string? SendGridApiKey { get; set; }
    public string? SendGridUrl { get; set; }
    public string FeedbackAddress { get; set; } = "";
    public string NoReplyAddress { get; set; } = "noreply@sagberg.net";
    public string SmtpServer { get; set; } = "";
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = "";
    public string SmtpPassword { get; set; } = "";
}
