using System.Data.Common;
using System.Linq;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Gaver.Data;
using Gaver.TestUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace Gaver.Web.Tests;

public class CustomWebApplicationFactory : ClaimInjectorWebApplicationFactory<IStartupAssembly>
{
    private DbConnection dbConnection;
    public ITestOutputHelper TestOutputHelper { get; set; }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        dbConnection = DbUtils.CreateInMemoryDbConnection();
        builder
            .UseEnvironment("Test")
            .ConfigureServices(services => {
                var descriptor = services.Single(s => s.ServiceType == typeof(DbContextOptions<GaverContext>));
                services.Remove(descriptor);
                services.AddDbContext<GaverContext>(options => options.UseSqlite(dbConnection));
            });

        return base.CreateHost(builder);
    }

    protected override void Dispose(bool disposing)
    {
        dbConnection.Close();
    }

    public void ResetDatabase()
    {
        dbConnection.Close();
        dbConnection.Open();
        var context = Services.GetRequiredService<GaverContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
