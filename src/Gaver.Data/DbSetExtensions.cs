using Microsoft.EntityFrameworkCore;

namespace Gaver.Data
{
    public static class DbContextExtensions
    {
        public static void Delete<T>(this DbContext context, int id) where T : class, IEntityWithId, new()
        {
            var entity = new T
            {
                Id = id
            };
            context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Deleted;
        }
    }
}