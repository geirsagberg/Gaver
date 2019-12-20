using System;
using System.Linq;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Gaver.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Gaver.Web.Tests
{
    public class CustomWebApplicationFactory : ClaimInjectorWebApplicationFactory<Startup>
    {
        private string databaseName = "default";

        public ITestOutputHelper TestOutputHelper { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder
                .UseEnvironment("Test")
                .ConfigureAppConfiguration(config => config.AddUserSecrets<Startup>())
                .ConfigureServices(services => {
                    var descriptor =
                        services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<GaverContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<GaverContext>(options => { options.UseInMemoryDatabase(databaseName); });
                })
                ;
        }

        public void ResetDatabase()
        {
            databaseName = Guid.NewGuid().ToString();
        }
    }
}
