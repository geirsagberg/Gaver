using System.Linq;
using Gaver.TestUtils;
using Xunit;
using Gaver.Common.Extensions;
using MediatR;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Gaver.Web.Tests
{
    public class RequestHandlerTests : WebTestBase
    {
        public RequestHandlerTests(CustomWebApplicationFactory webAppFactory) : base(webAppFactory)
        {
        }

        [Fact]
        public void All_requests_have_handlers()
        {
            var requestTypes = typeof(Startup).Assembly.ExportedTypes.Where(t => t.Implements<IBaseRequest>());
            var genericType = typeof(IRequestHandler<,>);

            var handlerTypes = requestTypes.Select(t => {
                var requestInterface = t.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));
                var responseType = requestInterface != null ? requestInterface.GenericTypeArguments[0] : typeof(Unit);
                var specificRequestType = genericType.MakeGenericType(t, responseType);
                var handler = ServiceProvider.GetService(specificRequestType);
                return (t, handler);
            });

            using (new AssertionScope()) {
                foreach (var (request, handler) in handlerTypes) {
                    handler.Should().NotBeNull("{0} should have a handler", request.Name);
                }
            }
        }
    }
}
