using System.Reflection;
using Gaver.Common.Utils;
using Gaver.Web.Features.Mail;
using LightInject;
using Xunit;

namespace Gaver.Web.Tests
{
    public class MapperServiceTests
    {
        [Fact]
        public void Mapping_is_valid()
        {
            var container = new ServiceContainer(new ContainerOptions {
                EnableVariance = false,
                EnablePropertyInjection = false
            });
            container.RegisterAssembly(typeof(Startup).GetTypeInfo().Assembly);
            container.RegisterAssembly(typeof(MailMappingProfile).GetTypeInfo().Assembly);
            container.RegisterInstance(Mocks.GetMockHttpContextAccessor());

            var service = container.Create<MapperService>();

            service.ValidateMappings();
        }
    }
}
