using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Gaver.Data.Tests
{
    public class NormalizrSchemaGeneratorTests
    {
        private readonly GaverContext context;

        public NormalizrSchemaGeneratorTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlite()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<GaverContext>()
                .UseSqlite("Data Source=:memory:;Version=3;New=True;")
                .UseInternalServiceProvider(serviceProvider);

            context = new GaverContext(builder.Options);
        }

        // [Fact]
        // public void Can_generate_normalizr_schema()
        // {
        //     var model = context.Model;

        //     var output = NormalizrSchemaGenerator.GetSchema(model);

        //     output.Should().Be("import { Schema, arrayOf } from 'normalizr'\n"
        //         + "export const ChatMessage = new Schema('chatMessages')\n"
        //         + "export const User = new Schema('users')\n"
        //         + "export const Wish = new Schema('wishes')\n"
        //         + "export const WishList = new Schema('wishLists')\n");
        // }

        // [Fact]
        // public void Can_generate_normalizr_schema_json() {
        //     var model = context.Model;

        //     var output = NormalizrSchemaGenerator.GetSchemaJson(model);
        // }
    }
}
