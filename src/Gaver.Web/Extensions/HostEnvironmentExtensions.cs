using Microsoft.Extensions.Hosting;

namespace Gaver.Web.Extensions
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsTest(this IHostEnvironment hostEnvironment) => hostEnvironment.IsEnvironment("Test");
    }
}
