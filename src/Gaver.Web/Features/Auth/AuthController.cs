using System.Threading.Tasks;
using Gaver.Web.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gaver.Web.Features.Auth;

public class AuthController : GaverControllerBase
{
    private readonly Auth0Settings auth0Settings;

    public AuthController(Auth0Settings auth0Settings) => this.auth0Settings = auth0Settings;

    [HttpGet]
    [AllowAnonymous]
    public async Task<AuthSettingsDto> GetAuthSettings()
        => new(auth0Settings.ClientId, auth0Settings.Domain, auth0Settings.Audience);
}

public record AuthSettingsDto(string ClientId, string Domain, string Audience);