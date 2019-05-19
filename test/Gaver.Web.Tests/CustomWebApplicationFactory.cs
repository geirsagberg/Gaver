using AspNetCore.Testing.Authentication.ClaimInjector;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Gaver.Web.Tests
{
    public class CustomWebApplicationFactory : ClaimInjectorWebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder
                .UseEnvironment("Test")
                .ConfigureAppConfiguration(config => config.AddUserSecrets<Startup>())
                ;
        }
    }
}
