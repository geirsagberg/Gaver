using Gaver.Common.Contracts;
using Gaver.Data;
using Gaver.Web;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Process {ProcessId} started", Environment.ProcessId);

try {
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, config) => config
        .WriteTo.Console()
        .ReadFrom.Configuration(context.Configuration));

    if (builder.Configuration.GetConnectionString("AppConfig") is { } connectionString && connectionString.IsNotEmpty())
        builder.Configuration.AddAzureAppConfiguration(connectionString);

    builder.ConfigureServices();

    var app = builder.Build();

    app.Services.GetRequiredService<IMapperService>().ValidateMappings();

    using (var scope = app.Services.CreateScope()) {
        var context = scope.ServiceProvider.GetRequiredService<GaverContext>();
        context.Database.Migrate();
    }

    if (app.Configuration.GetConnectionString("AppConfig").IsNotEmpty())
        app.UseAzureAppConfiguration();

    app.SetupPipeline();

    app.Run();
} catch (Exception exception) {
    if (!exception.GetType().Name.EndsWith("StopTheHostException"))
        Log.Fatal(exception, "Unhandled exception");
} finally {
    Log.Information("Shut down {ProcessId} complete", Environment.ProcessId);
    Log.CloseAndFlush();
}
