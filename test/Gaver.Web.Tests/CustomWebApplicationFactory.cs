using System.Data.Common;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Gaver.Data;
using Gaver.TestUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Gaver.Web.Tests;

public class CustomWebApplicationFactory : ClaimInjectorWebApplicationFactory<IStartupAssembly> {
    private DbConnection dbConnection;
    public ITestOutputHelper TestOutputHelper { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        base.ConfigureWebHost(builder);

        dbConnection = DbUtils.CreateInMemoryDbConnection();

        builder.UseEnvironment("Test");
        builder.ConfigureServices(services => {
            services.AddDbContext<GaverContext>(options => options.UseSqlite(dbConnection));
        });
    }

    protected override void Dispose(bool disposing) => dbConnection.Close();

    public void ResetDatabase() {
        dbConnection.Close();
        dbConnection.Open();
        var context = Services.GetRequiredService<GaverContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
