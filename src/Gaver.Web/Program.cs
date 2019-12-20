using System;
using System.Diagnostics;
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
                Log.Information("Process {ProcessId} started", Process.GetCurrentProcess().Id);
                var host = CreateHostBuilder(args)
                    .Build();

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

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .ConfigureAppConfiguration(config => config.AddUserSecrets<Startup>())
            .UseSerilog();
    }
}
