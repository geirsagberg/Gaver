using Xunit;

namespace Gaver.Logic.Tests
{
    public class MapperServiceTests
    {
        [Fact]
        public void Mapping_is_valid() {
            var service = new MapperService();

            service.ValidateMappings();
        }
    }
}