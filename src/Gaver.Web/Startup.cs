using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gaver.Common.Contracts;
using Gaver.Common.Extensions;
using Gaver.Common.Utils;
using Gaver.Data;
using Gaver.Web.CrossCutting;
using Gaver.Web.Exceptions;
using Gaver.Web.Extensions;
using Gaver.Web.Hubs;
using Gaver.Web.Options;
using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;

namespace Gaver.Web
{
    public class Startup
    {
        private readonly IHostEnvironment hostEnvironment;
        private readonly List<string> missingOptions = new();

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ScanAssemblies();

            services.AddCustomAuth(Configuration);

            services.AddCustomMvc();
            services.AddCustomSwagger(Configuration);
            services.AddCustomDbContext(Configuration);
            services.AddCustomHealthChecks(Configuration, hostEnvironment);
            services.AddFeatureManagement(Configuration.GetSection("Features"));

            services.AddSingleton<IMapperService, MapperService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSignalR();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddProblemDetails();
            services.AddValidationProblemDetails();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(AuthenticationPreProcessor<>));
            services.AddScoped(typeof(IRequestPreProcessor<>), typeof(SharedListRequestPreProcessor<>));
            services.AddScoped(typeof(IRequestPreProcessor<>), typeof(MyWishRequestPreProcessor<>));

            ConfigureOptions(services);

            if (hostEnvironment.IsProduction()) {
                services.Configure<HttpsRedirectionOptions>(options => options.HttpsPort = 443);
            }
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
            services.AddScoped(provider => provider.GetRequiredService<IOptionsSnapshot<T>>().Value);
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                SetupForDevelopment(app, env);
            } else {
                SetupForProduction(app);
            }

            SetupStaticFiles(app, env);
            app.UseRouting();

            app.UseWhen(IsJsonRequest, app2 => app2.UseProblemDetails());

            app.UseHttpException();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthClientId(Configuration["auth0:ClientId"]);
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> {
                    {"audience", Configuration["auth0:Audience"]}
                });
            });

            app.UseEndpoints(endpoints => {
                if (!hostEnvironment.IsTest()) {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions {
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                }

                endpoints.MapHub<ListHub>("/hub");
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute("API 404", "api/{*anything}", new {
                    controller = "Error",
                    action = "NotFound"
                });
            });

            app.UseSpa(_ => {});
        }

        private static void SetupStaticFiles(IApplicationBuilder app, IHostEnvironment env)
        {
            var cachePeriod = (int) (env.IsDevelopment()
                ? TimeSpan.FromMinutes(10).TotalSeconds
                : TimeSpan.FromDays(365).TotalSeconds);
            var staticFileOptions = new StaticFileOptions {
                OnPrepareResponse = ctx =>
                    ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}")
            };
            app.UseDefaultFiles();
            app.UseStaticFiles(staticFileOptions);
        }

        private static bool IsJsonRequest(HttpContext context)
        {
            var requestHeaders = context.Request.GetTypedHeaders();
            return requestHeaders.Accept.EmptyIfNull().Any(h => h.MediaType == "application/json") ||
                requestHeaders.ContentType?.MediaType == "application/json";
        }

        private static void SetupForProduction(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
#if !DEBUG
            app.UseHsts();
#endif
        }

        private static void SetupForDevelopment(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
        }
    }
}
