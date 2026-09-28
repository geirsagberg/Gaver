namespace Gaver.Web.Features.Mail;

public static class MailMappings {
    public static SendGridMail ToSendGridMail(MailModel mail) => new() {
        Subject = mail.Subject,
        From = new SendGridAddress {
            Email = mail.From,
            Name = "Gaver"
        },
        Content = new[] {
            new SendGridContent {
                Value = mail.Content,
                Type = "text/html"
            }
        },
        Personalizations = new[] {
            new SendGridPersonalization {
                To = mail.To.Select(to => new SendGridAddress {
                    Email = to
                }).ToList()
            }
        }
    };
}
