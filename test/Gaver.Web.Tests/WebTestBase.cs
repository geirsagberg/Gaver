using System;
using System.Net.Http;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Gaver.Web.Tests
{
    [Trait("Category", "WebTest")]
    public abstract class WebTestBase : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly ClaimInjectorHandlerHeaderConfig RoleConfig;
        private readonly IServiceScope serviceScope;

        protected WebTestBase(CustomWebApplicationFactory webAppFactory)
        {
            Client = webAppFactory.CreateClient();
            serviceScope = webAppFactory.Services.CreateScope();
            RoleConfig = webAppFactory.RoleConfig;
        }

        protected IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        protected HttpClient Client { get; }
    }
}
