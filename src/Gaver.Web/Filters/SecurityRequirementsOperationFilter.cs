using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gaver.Web.Filters;

public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
        var requireAuthorization = filterPipeline.Select(filterInfo => filterInfo.Filter)
            .Any(filter => filter is AuthorizeFilter);
        var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter)
            .Any(filter => filter is IAllowAnonymousFilter);

        if (!requireAuthorization || allowAnonymous) return;

        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        if (operation.Security == null) operation.Security = new List<OpenApiSecurityRequirement>();

        operation.Responses.Add("401", new OpenApiResponse {Description = "Unauthorized"});
        operation.Responses.Add("403", new OpenApiResponse {Description = "Forbidden"});

        operation.Security.Add(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Id = "oauth2",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                new[] {"openid profile email"}
            }
        });
    }
}