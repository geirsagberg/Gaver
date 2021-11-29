using Microsoft.IdentityModel.Tokens;
using Gaver.Common.Extensions;
using System.Text;

namespace Gaver.Web.Options;

public class Auth0Settings
{
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
    public string Domain { get; init; }
    public string Audience { get; init; }

    public SymmetricSecurityKey? SigningKey => ClientSecret.IsNullOrEmpty() ? null : new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ClientSecret));
}