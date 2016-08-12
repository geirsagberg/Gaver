using Xunit;
using Gaver.Logic;
using AutoMapper;

namespace Gaver.Logic.Tests
{
    public class MapperServiceTests
    {
        [Fact]
        public void Mapping_is_valid() {
            var service = new MapperService(new Profile[0]);

            service.ValidateMappings();
        }
    }
}