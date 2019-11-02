using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gaver.Web.Filters
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is IAllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous) {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                if (operation.Security == null) {
                    operation.Security = new List<OpenApiSecurityRequirement>();
                }

                operation.Security.Add(new OpenApiSecurityRequirement {
                    {new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Id = "oauth2",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new[] {"openid profile email"}}
                });
            }
        }
    }
}
