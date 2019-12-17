using System;
using System.Net.Http;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests
{
    [Trait("Category", "WebTest")]
    public abstract class WebTestBase : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly Lazy<HttpClient> clientLazy;
        protected readonly ClaimInjectorHandlerHeaderConfig RoleConfig;
        private readonly IServiceScope serviceScope;

        protected WebTestBase(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper)
        {
            clientLazy = new Lazy<HttpClient>(webAppFactory.CreateClient);
            serviceScope = webAppFactory.Services.CreateScope();
            RoleConfig = webAppFactory.RoleConfig;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.TestOutput(testOutputHelper)
                .CreateLogger();
        }

        protected IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        protected HttpClient Client => clientLazy.Value;
    }
}
