namespace Gaver.Web.Options
{
    public class MailOptions
    {
        public string SendGridApiKey { get; set; }
        public string SendGridUrl { get; set; }
        public string FeedbackAddress { get; set; }
        public string NoReplyAddress { get; set; }
    }
}
