using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
using Gaver.Common.Extensions;
using MediatR;
using Xunit;
using Xunit.Abstractions;

namespace Gaver.Web.Tests;

public class RequestHandlerTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper) : WebTestBase(webAppFactory, testOutputHelper) {
    [Fact]
    public void All_requests_have_handlers() {
        var requestTypes = typeof(IStartupAssembly).Assembly.ExportedTypes.Where(t => t.Implements<IBaseRequest>());
        var genericType = typeof(IRequestHandler<>);
        var genericTypeWithResponse = typeof(IRequestHandler<,>);

        var handlerTypes = requestTypes.Select(t => {
            var requestInterface = t.GetInterfaces().FirstOrDefault(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));
            var responseType = requestInterface != null ? requestInterface.GenericTypeArguments[0] : typeof(Unit);
            var specificRequestType = responseType == typeof(Unit) ? genericType.MakeGenericType(t) : genericTypeWithResponse.MakeGenericType(t, responseType);
            var handler = ServiceProvider.GetService(specificRequestType);
            return (t, handler);
        });

        using (new AssertionScope()) {
            foreach (var (request, handler) in handlerTypes)
                handler.Should().NotBeNull("{0} should have a handler", request.Name);
        }
    }
}
