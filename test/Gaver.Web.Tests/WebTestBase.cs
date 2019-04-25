using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Gaver.Web.Tests
{
    public abstract class WebTestBase : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly IServiceScope serviceScope;

        protected WebTestBase(CustomWebApplicationFactory webAppFactory)
        {
            Client = webAppFactory.CreateClient();
            serviceScope = webAppFactory.Server.Host.Services.CreateScope();
        }

        protected IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

        protected HttpClient Client { get; }
    }
}
