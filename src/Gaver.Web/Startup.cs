using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AutoMapper;
using Gaver.Common;
using Gaver.Common.Contracts;
using Gaver.Common.Extensions;
using Gaver.Common.Utils;
using Gaver.Data;
using Gaver.Web.CrossCutting;
using Gaver.Web.Exceptions;
using Gaver.Web.Hubs;
using Gaver.Web.Options;
using JetBrains.Annotations;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

[assembly: AspMvcViewLocationFormat(@"~\Features\{1}\{0}.cshtml")]
[assembly: AspMvcViewLocationFormat(@"~\Features\Shared\{0}.cshtml")]

namespace Gaver.Web
{
    public class Startup
    {
        private readonly List<string> missingOptions = new List<string>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        private static void UseRootNodeModules(IHostingEnvironment hostingEnvironment)
        {
            var nodeDir = Path.Combine(hostingEnvironment.ContentRootPath, "../../node_modules");
            Environment.SetEnvironmentVariable("NODE_PATH", nodeDir);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Scan(scan => {
                scan.FromAssemblyOf<ICommonAssembly>().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromEntryAssembly().AddClasses().AsImplementedInterfaces().WithTransientLifetime();
                scan.FromEntryAssembly().AddClasses().AsSelf().WithTransientLifetime();

                scan.FromEntryAssembly().AddClasses(classes => classes.AssignableTo<Profile>()).As<Profile>()
                    .WithSingletonLifetime();
            });

            services.AddCustomAuth(Configuration);

            services.AddCustomMvc();
            services.AddCustomSwagger();
            services.AddCustomDbContext(Configuration);

            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR();
            services.AddMediatR();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(AuthenticationPreProcessor<>));

            ConfigureOptions(services);
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.AddOptions();

            ConfigureOptions<MailOptions>(services, "mail");
            ConfigureOptions<Auth0Settings>(services, "auth0");

            if (missingOptions.Any()) {
                throw new Exception("Missing settings: " + missingOptions.ToJoinedString());
            }
        }

        private void ConfigureOptions<T>(IServiceCollection services, string key) where T : class, new()
        {
            var configurationSection = Configuration.GetSection(key);
            var options = configurationSection.Get<T>();

            var missing = typeof(T)
                .GetProperties()
                .Where(propertyInfo => propertyInfo.GetValue(options).ToStringOrEmpty().IsNullOrEmpty())
                .Select(propertyInfo => $"{key}:{propertyInfo.Name}");

            missingOptions.AddRange(missing);

            services.Configure<T>(configurationSection);

            // Enable injection of updated strongly typed options
            services.AddScoped(provider => provider.GetService<IOptionsSnapshot<T>>().Value);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                SetupForDevelopment(app, loggerFactory, env);
            } else {
                SetupForProduction(app, loggerFactory);
            }

            app.UseFileServer();

            app.UseHttpException();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseSignalR(routes => routes.MapHub<ListHub>("/listHub"));

            SetupRoutes(app);
        }

        private static void SetupRoutes(IApplicationBuilder app)
        {
            app.UseMvc(routes => {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("API 404", "api/{*anything}", new {
                    controller = "Error",
                    action = "NotFound"
                });
                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new {
                        controller = "Home",
                        action = "Index"
                    });
            });
        }

        [Conditional("RELEASE")]
        private static void SetupForProduction(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseHttpsRedirection();
            app.UseHsts();
        }

        private static void SetupForDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory,
            IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            UseRootNodeModules(env);

            app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                HotModuleReplacement = true,
                HotModuleReplacementClientOptions = new Dictionary<string, string> {
                    {"reload", "true"}
                }
            });
        }
    }
}
