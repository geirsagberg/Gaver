using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Gaver.Data;
using Gaver.Logic;
using Gaver.Logic.Contracts;
using Gaver.Logic.Extensions;
using Gaver.Logic.Services;
using Gaver.Web.Exceptions;
using Gaver.Web.Extensions;
using Gaver.Web.Utils;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
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
        private readonly List<string> missingOptions = new List<string>();

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

            ConfigureOptions<MailOptions>(services, "mail");
            ConfigureOptions<Auth0Settings>(services, "auth0");
            if (missingOptions.Any())
                throw new Exception("Missing settings: " + missingOptions.ToJoinedString());

            services.AddMvc(o => {
                o.Filters.Add(new CustomExceptionFilterAttribute());
                o.Filters.Add(new ValidationAttribute());
                o.UseFromBodyBinding();
            }); //.AddControllersAsServices();
            var connectionString = Configuration.GetConnectionString("GaverContext");
            if (connectionString.IsNullOrEmpty())
                throw new ConfigurationException("ConnectionStrings:GaverContext");
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<GaverContext>(options => options
                    .UseNpgsql(connectionString, b => b
                        .MigrationsAssembly(GetType().GetTypeInfo().Assembly.FullName)), ServiceLifetime.Transient);

            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR(options => options.RegisterInvocationAdapter<CustomJsonNetInvocationAdapter>("json"));
            services.AddSingleton<IConfigureOptions<SignalROptions>, CustomSignalROptionsSetup>();
            services.AddSwaggerGen(options => { options.OperationFilter<AuthorizationHeaderParameterOperationFilter>(); });
            services.AddSingleton(factory => new JsonSerializer {
                ContractResolver = new SignalRContractResolver()
            });
            services.AddMediatR();

            var container = new ServiceContainer(new ContainerOptions {
                EnablePropertyInjection = false,
                EnableVariance = false,
                LogFactory = type => entry => Log.Logger.ForContext(type).Write(entry.Level.ToSerilogEventLevel(), entry.Message)
            });
            container.ScopeManagerProvider = new StandaloneScopeManagerProvider();
            container.RegisterAssembly<ILogicAssembly>();
            container.RegisterAssembly<Startup>();

            return container.CreateServiceProvider(services);
        }

        private void ConfigureOptions<T>(IServiceCollection services, string key) where T : class
        {
            var configurationSection = Configuration.GetSection(key);
            var options = configurationSection.Get<T>();

            var missing = typeof(T)
                .GetProperties()
                .Where(propertyInfo => propertyInfo.GetValue(options).IsNullOrDefault())
                .Select(propertyInfo => $"{key}:{propertyInfo.Name}");

            missingOptions.AddRange(missing);

            services.Configure<T>(configurationSection);
            services.AddScoped(provider => provider.GetService<IOptionsSnapshot<T>>().Value);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env,
            Auth0Settings auth0Settings)
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
            app.UseJwtAuthentication(auth0Settings);

            app.UseFileServer();
            app.UseSignalR(routes => routes.MapHub<ListHub>("/listHub"));

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