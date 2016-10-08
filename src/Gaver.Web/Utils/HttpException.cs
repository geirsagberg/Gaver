using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Gaver.Web.Utils
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
        {
            return application.UseMiddleware<HttpExceptionMiddleware>();
        }
    }

    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (HttpException httpException)
            {
                context.Response.StatusCode = httpException.StatusCode;
                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = httpException.Message;
            }
        }
    }

    public class HttpException : Exception
    {
        public HttpException(int httpStatusCode)
        {
            this.StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode)
        {
            this.StatusCode = (int) httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message) : base(message)
        {
            this.StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            this.StatusCode = (int) httpStatusCode;
        }

        public HttpException(int httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            this.StatusCode = httpStatusCode;
        }

        public HttpException(HttpStatusCode httpStatusCode, string message, Exception inner) : base(message, inner)
        {
            this.StatusCode = (int) httpStatusCode;
        }

        public int StatusCode { get; }
    }
}