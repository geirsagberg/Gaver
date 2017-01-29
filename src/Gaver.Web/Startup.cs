using System;
using System.IO;
using System.Reflection;
using Gaver.Data;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using Gaver.Logic.Services;
using Gaver.Web.Exceptions;
using Gaver.Web.Filters;
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
using Microsoft.Extensions.Options;
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
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("config.json", false, true);

            builder.AddEnvironmentVariables();
            if (hostingEnvironment.IsDevelopment()) {
                builder.AddUserSecrets();
                UseRootNodeModules(hostingEnvironment);
            }

            Configuration = builder.Build();
        }

        private IConfiguration Configuration { get; }

        private static void UseRootNodeModules(IHostingEnvironment hostingEnvironment)
        {
            var nodeDir = Path.Combine(hostingEnvironment.ContentRootPath, "../../node_modules");
            Environment.SetEnvironmentVariable("NODE_PATH", nodeDir);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddAuthentication();
            services.AddAuthorization();
            services.Configure<MailOptions>(Configuration.GetSection("mail"));
            services.Configure<Auth0Settings>(Configuration.GetSection("auth0"));

            services.AddMvc(o => {
                o.Filters.Add(new CustomExceptionFilterAttribute());
                o.Filters.Add(new ValidationAttribute());
                o.UseFromBodyBinding();
            });
            var connectionString = Configuration.GetConnectionString("GaverContext");
            if (connectionString.IsNullOrEmpty())
                throw new ConfigurationException("ConnectionStrings:GaverContext");
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<GaverContext>(options => options
                    .UseNpgsql(connectionString, b => b
                        .MigrationsAssembly(GetType().GetTypeInfo().Assembly.FullName)), ServiceLifetime.Transient);

            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR(options => { options.Hubs.EnableDetailedErrors = true; });
            services.AddSwaggerGen(options => { options.OperationFilter<AuthorizationHeaderParameterOperationFilter>(); });
            services.AddSingleton(factory => new JsonSerializer {
                ContractResolver = new SignalRContractResolver()
            });

            var container = new ServiceContainer(new ContainerOptions {
                EnablePropertyInjection = false,
                EnableVariance = false,
                LogFactory = type => entry => Log.Logger.ForContext(type).Write(entry.Level.ToSerilogEventLevel(), entry.Message)
            });
            container.ScopeManagerProvider = new StandaloneScopeManagerProvider();
            container.RegisterAssembly<ILogicAssembly>();
            container.RegisterAssembly<Startup>();
            var provider = container.CreateServiceProvider(services);

            return provider;
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env,
            IOptions<Auth0Settings> auth0SettingsOptions)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(env.IsDevelopment() ? LogEventLevel.Debug : LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            var filterLoggerSettings = new FilterLoggerSettings {
                {"Microsoft.EntityFrameworkCore", LogLevel.Information},
                {"Microsoft.AspNetCore.NodeServices", LogLevel.Information},
                {"Microsoft.AspNetCore.SignalR", LogLevel.Information},
                {"Microsoft.AspNetCore.Authentication", LogLevel.Information},
                {"Microsoft", LogLevel.Warning},
                {"System", LogLevel.Warning}
            };
            if (env.IsDevelopment()) {
                loggerFactory
                    .WithFilter(filterLoggerSettings)
                    .AddConsole(LogLevel.Debug);

                app.UseDeveloperExceptionPage();

                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            } else {
                loggerFactory
                    .WithFilter(filterLoggerSettings)
                    .AddConsole(LogLevel.Information);
            }
            var auth0Settings = auth0SettingsOptions.Value;
            app.UseJwtAuthentication(auth0Settings);

            app.UseFileServer();
            app.UseWebSockets();
            app.UseSignalR();

            app.UseSwagger();
            app.UseSwaggerUi();

            app.UseHttpException();
            app.UseMvc(routes => {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("API 404", "api/{*anything}", new {controller = "Error", action = "NotFound"});
                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new {controller = "Home", action = "Index"});
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

            using (var context = host.Services.GetRequiredService<GaverContext>()) {
                context.Database.Migrate();
            }

            host.Run();
        }
    }
}