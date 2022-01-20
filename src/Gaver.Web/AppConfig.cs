using Gaver.Common.Extensions;
using Gaver.Web.Exceptions;
using Gaver.Web.Extensions;
using Gaver.Web.Hubs;
using Gaver.Web.Options;
using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Gaver.Web;

public static class AppConfig
{
    public static void SetupStaticFiles(this WebApplication app)
    {
        var cachePeriod = (int) (app.Environment.IsDevelopment()
            ? TimeSpan.FromMinutes(10).TotalSeconds
            : TimeSpan.FromDays(365).TotalSeconds);
        var staticFileOptions = new StaticFileOptions {
            OnPrepareResponse = ctx =>
                ctx.Context.Response.Headers.Append("Cache-Control", $"public, max-age={cachePeriod}")
        };
        app.UseDefaultFiles();
        app.UseStaticFiles(staticFileOptions);
    }

    public static void SetupPipeline(this WebApplication app)
    {
        if (app.Environment.IsProduction()) {
            app.UseHttpsRedirection();
#if !DEBUG
        app.UseHsts();
#endif
        }

        app.SetupStaticFiles();
        app.UseRouting();

        app.UseWhen(IsJsonRequest, app2 => app2.UseProblemDetails());

        app.UseHttpException();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseAzureAppConfiguration();

        app.UseSwagger();
        app.UseSwaggerUI(c => {
            var auth0Settings = app.Services.GetRequiredService<Auth0Settings>();
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.OAuthClientId(auth0Settings.ClientId);
            c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> {
                { "audience", auth0Settings.Audience }
            });
        });

        app.UseEndpoints(endpoints => {
            if (!app.Environment.IsTest())
                endpoints.MapHealthChecks("/health", new HealthCheckOptions {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

            endpoints.MapHub<ListHub>("/hub");
            endpoints.MapDefaultControllerRoute();
            endpoints.MapControllerRoute("API 404", "api/{*anything}", new {
                controller = "Error",
                action = "NotFound"
            });
        });

        app.UseSpa(_ => { });
    }

    private static bool IsJsonRequest(HttpContext context)
    {
        var requestHeaders = context.Request.GetTypedHeaders();
        return requestHeaders.Accept.EmptyIfNull().Any(h => h.MediaType == "application/json") ||
            requestHeaders.ContentType?.MediaType == "application/json";
    }
}
