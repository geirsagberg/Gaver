using System.Collections.Generic;

namespace Gaver.Web.Features.Mail
{
    public class SendGridPersonalization
    {
        public IList<SendGridAddress> To { get; set; } = new List<SendGridAddress>();
    }
}