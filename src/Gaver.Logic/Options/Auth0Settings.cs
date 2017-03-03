using System.Text;
using Microsoft.IdentityModel.Tokens;
using Gaver.Logic.Extensions;

namespace Gaver.Logic
{
    public class Auth0Settings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }

		public SymmetricSecurityKey SigningKey => ClientSecret.IsNullOrEmpty() ? null : new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ClientSecret));
    }
}