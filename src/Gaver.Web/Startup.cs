using System;
using System.IO;
using System.Reflection;
using Gaver.Data;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using Gaver.Logic.Services;
using Gaver.Web.Utils;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using WebApiContrib.Core;
using WebApiContrib.Core.Filters;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Gaver.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("config.json");

            if (hostingEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
            var nodeDir = Path.Combine(hostingEnvironment.ContentRootPath, "../../node_modules");
            Environment.SetEnvironmentVariable("NODE_PATH", nodeDir);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthentication();
            services.AddAuthorization();
            services.Configure<MailOptions>(Configuration.GetSection("mail"));

            services.AddMvc(o =>
            {
                o.Filters.Add(new CustomExceptionFilterAttribute());
                o.Filters.Add(new ValidationAttribute());
                o.UseFromBodyBinding();
            });
            const string connectionString = "Data Source=MyDb.db";
            services.AddEntityFrameworkSqlite()
                .AddDbContext<GaverContext>(options => options
                    .UseSqlite(connectionString, b => b
                        .MigrationsAssembly(GetType().GetTypeInfo().Assembly.FullName)), ServiceLifetime.Transient);

            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR(options => { options.Hubs.EnableDetailedErrors = true; });
            services.AddSwaggerGen();
            services.AddSingleton(factory => new JsonSerializer
            {
                ContractResolver = new SignalRContractResolver()
            });

            var container = new ServiceContainer
            {
                PropertyDependencySelector = new PropertyInjectionDisabler()
            };
            container.RegisterAssembly<ILogicAssembly>();
            container.RegisterAssembly<Startup>();
            var provider = container.CreateServiceProvider(services);

            return provider;
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(env.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            if (env.IsDevelopment())
            {
                loggerFactory
                    .WithFilter(new FilterLoggerSettings
                    {
                        {"Microsoft.EntityFrameworkCore", LogLevel.Information},
                        {"Microsoft.AspNetCore.NodeServices", LogLevel.Information},
                        {"Microsoft.AspNetCore.SignalR", LogLevel.Information},
                        {"Microsoft", LogLevel.Warning},
                        {"System", LogLevel.Warning}
                    })
                    .AddConsole(LogLevel.Debug);

                app.UseDeveloperExceptionPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = false
            });

            app.UseFileServer();
            app.UseWebSockets();
            app.UseSignalR();

            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("API 404", "api/{*anything}", new {controller = "Error", action = "NotFound"});
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new {controller = "Home", action = "Index"});
            });
        }

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();

            host.Services.GetRequiredService<IMapperService>().ValidateMappings();

            using (var context = host.Services.GetRequiredService<GaverContext>())
            {
                context.Database.EnsureCreated();
            }

            host.Run();
        }
    }
}