using System;
using System.Net.Http;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Gaver.Data;
using Gaver.Data.Entities;
using Gaver.Web.MvcUtils;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests;

[Trait("Category", "WebTest")]
[Collection("WebTests")]
public abstract class WebTestBase : IClassFixture<CustomWebApplicationFactory>
{
    private readonly Lazy<HttpClient> clientLazy;
    protected readonly ClaimInjectorHandlerHeaderConfig RoleConfig;
    private readonly IServiceScope serviceScope;

    protected WebTestBase(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper)
    {
        webAppFactory.TestOutputHelper = testOutputHelper;
        clientLazy = new Lazy<HttpClient>(webAppFactory.CreateClient);
        serviceScope = webAppFactory.Services.CreateScope();
        RoleConfig = webAppFactory.RoleConfig;

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.TestOutput(testOutputHelper)
            .CreateLogger();

        webAppFactory.ResetDatabase();
    }

    protected GaverContext GaverContext => ServiceProvider.GetRequiredService<GaverContext>();

    protected IServiceProvider ServiceProvider => serviceScope.ServiceProvider;

    protected HttpClient Client => clientLazy.Value;

    protected void SetAuthenticatedUser(int userId = 1)
    {
        RoleConfig.Reset();
        RoleConfig.AddClaim(GaverClaimTypes.GaverUserId, userId.ToString());
    }

    protected void SetAuthenticatedUser(User user)
    {
        SetAuthenticatedUser(user.Id);
    }
}
