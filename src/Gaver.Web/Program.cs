using Gaver.Data;
using Gaver.Logic.Contracts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Gaver.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            host.Services.GetRequiredService<IMapperService>().ValidateMappings();

            using (var scope = host.Services.CreateScope()) {
                var context = scope.ServiceProvider.GetRequiredService<GaverContext>();
                context.Database.Migrate();
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            .UseStartup<Startup>()
            .ConfigureLogging(ConfigureLogging)
            .Build();

        private static void ConfigureLogging(ILoggingBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();
            builder.AddSerilog();
        }
    }
}