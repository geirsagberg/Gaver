using System;
using Gaver.Common.Contracts;
using Gaver.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
                var host = CreateWebHostBuilder(args)
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

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            .UseStartup<Startup>()
            .UseSerilog();
    }
}
