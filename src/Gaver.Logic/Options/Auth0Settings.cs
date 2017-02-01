using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Gaver.Logic
{
    public class Auth0Settings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Domain { get; set; }

        public SymmetricSecurityKey SigningKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ClientSecret));
    }
}