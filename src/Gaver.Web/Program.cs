using System;
using Gaver.Common.Contracts;
using Gaver.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Gaver.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try {
                Log.Information("Process {ProcessId} started", Environment.ProcessId);
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
                var host = CreateHostBuilder(args, environment)
                    .Build();

                var environmentName = host.Services.GetRequiredService<IHostEnvironment>().EnvironmentName;
                Log.Information("Environment: {Environment}", environmentName);

                host.Services.GetRequiredService<IMapperService>().ValidateMappings();

                using (var scope = host.Services.CreateScope()) {
                    var context = scope.ServiceProvider.GetRequiredService<GaverContext>();
                    context.Database.Migrate();
                }

                host.Run();
                return 0;
            } catch (Exception e) {
                Log.Fatal(e, "Host terminated unexpectedly");
                return 1;
            } finally {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args, string environment) => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureAppConfiguration(config => { if (environment == "Development") { _ = config.AddUserSecrets<Startup>(); } })
            .UseSerilog();
    }
}
