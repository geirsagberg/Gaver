using System.Collections.Generic;

namespace Gaver.Logic
{
    public class SendGridMail
    {
        public IList<SendGridPersonalization> Personalizations { get; set; } = new List<SendGridPersonalization>();
        public string Subject { get; set; }
        public SendGridAddress From { get; set; }
        public IList<SendGridContent> Content { get; set; } = new List<SendGridContent>();
    }

    public class SendGridPersonalization
    {
        public IList<SendGridAddress> To { get; set; } = new List<SendGridAddress>();
        // public IList<SendGridAddress> Cc { get; set; } = new List<SendGridAddress>();
        // public IList<SendGridAddress> Bcc { get; set; } = new List<SendGridAddress>();
    }

    public class SendGridAddress
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class SendGridContent
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}