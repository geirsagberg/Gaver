using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Gaver.Data
{
    public class NormalizrSchemaGenerator
    {
        public NormalizrSchemaGenerator()
        {
        }

        public static string GetSchema(IModel model)
        {
            var builder = new StringBuilder();
            builder.AppendLine("import { Schema, arryaOf } from 'normalizr'");

            foreach (var entityType in model.GetEntityTypes().OrderBy(t => t.Name))
            {
                builder.AppendLine($"export const {entityType.ClrType.Name} = new Schema('{entityType.Relational().TableName.ToCamelCase()}')");
            }
            return builder.ToString();
        }
    }
}
