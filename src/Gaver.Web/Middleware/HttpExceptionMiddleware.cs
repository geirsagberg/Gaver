﻿using System.Text.Json;
using System.Threading.Tasks;
using Gaver.Web.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Gaver.Web.Middleware
{
    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public HttpExceptionMiddleware(RequestDelegate next) => this.next = next;

        public async Task Invoke(HttpContext context)
        {
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
}
