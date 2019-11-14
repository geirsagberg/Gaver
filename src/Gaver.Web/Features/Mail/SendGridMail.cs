using System.Collections.Generic;

namespace Gaver.Web.Features.Mail
{
    public class SendGridMail
    {
        public IList<SendGridPersonalization> Personalizations { get; set; } = new List<SendGridPersonalization>();
        public string? Subject { get; set; }
        public SendGridAddress From { get; set; } = new SendGridAddress();
        public IList<SendGridContent> Content { get; set; } = new List<SendGridContent>();
    }
}
