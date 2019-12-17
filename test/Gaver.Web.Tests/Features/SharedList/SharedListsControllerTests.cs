using Xunit.Abstractions;

namespace Gaver.Web.Tests.Features.SharedList
{
    public class SharedListsControllerTests : WebTestBase
    {
        public SharedListsControllerTests(CustomWebApplicationFactory webAppFactory, ITestOutputHelper testOutputHelper)
            : base(webAppFactory, testOutputHelper)
        {
        }
    }
}
