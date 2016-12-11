namespace Gaver.Logic
{
    public class MailOptions
    {
        public string SendGridApiKey { get; set; }
        public string SendGridUrl { get; set; }
    }

    public class Auth0Settings {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
    }
}