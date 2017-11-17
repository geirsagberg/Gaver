using System.Collections.Generic;

namespace Gaver.Logic.Features.Mail
{
    public class MailModel
    {
        public IList<string> To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}