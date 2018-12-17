using Microsoft.IdentityModel.Tokens;
using Gaver.Common.Extensions;
using System.Text;

namespace Gaver.Web.Options
{
    public class Auth0Settings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }
        public string Audience { get; set; }

		public SymmetricSecurityKey SigningKey => ClientSecret.IsNullOrEmpty() ? null : new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ClientSecret));
    }
}
