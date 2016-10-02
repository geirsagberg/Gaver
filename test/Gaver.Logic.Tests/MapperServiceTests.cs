using Xunit;
using AutoMapper;
using Gaver.Logic.Services;

namespace Gaver.Logic.Tests
{
    public class MapperServiceTests
    {
        [Fact]
        public void Mapping_is_valid()
        {
            var service = new MapperService(new Profile[0]);

            service.ValidateMappings();
        }
    }
}