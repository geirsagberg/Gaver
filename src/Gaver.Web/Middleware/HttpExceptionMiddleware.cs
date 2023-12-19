using System.Text.Json;
using Gaver.Web.Exceptions;

namespace Gaver.Web.Middleware;

internal class HttpExceptionMiddleware(RequestDelegate next) {
    private readonly RequestDelegate next = next;

    public async Task Invoke(HttpContext context) {
        try {
            await next(context);
        } catch (HttpException httpException) {
            context.Response.StatusCode = httpException.StatusCode;
            if (!context.Response.HasStarted) {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(new {
                        error = new {
                            message = httpException.Message
                        }
                    }));
            }
        }
    }
}
