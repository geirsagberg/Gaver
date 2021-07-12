using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Gaver.Web.MvcUtils
{
    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions options;
        private readonly JsonOptions jsonOptions;

        public CustomProblemDetailsFactory(IOptions<ApiBehaviorOptions> options, IOptions<JsonOptions> jsonOptions)
        {
            this.options = options.Value;
            this.jsonOptions = jsonOptions.Value;
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            statusCode ??= 500;

            var problemDetails = new ProblemDetails {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext,
            ModelStateDictionary modelStateDictionary,
            int? statusCode = null,
            string? title = null,
            string? type = null,
            string? detail = null,
            string? instance = null)
        {
            if (modelStateDictionary == null) {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= 400;

            var errors = modelStateDictionary.ToDictionary(
                kvp => jsonOptions.JsonSerializerOptions?.PropertyNamingPolicy?.ConvertName(kvp.Key) ?? kvp.Key,
                kvp => kvp.Value.Errors.Select(x => x.ErrorMessage).ToArray()
            );

            var problemDetails = new ValidationProblemDetails(errors) {
                Status = statusCode,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            if (title != null) {
                // For validation problem details, don't overwrite the default title with null.
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;

            if (options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData)) {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
            problemDetails.Extensions["traceId"] = traceId;
        }
    }
}
