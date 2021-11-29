using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gaver.Data.Contracts;
using Gaver.Data.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Gaver.Data;

public static class DbContextExtensions
{
    public static void Delete<T>(this DbContext context, int id) where T : class, IEntityWithId, new()
    {
        var entity = new T {
            Id = id
        };
        if (context.Entry(entity).State == EntityState.Detached) {
            context.Attach(entity);
        }

        context.Entry(entity).State = EntityState.Deleted;
    }

    public static T GetOrDie<T>(this DbContext context, int id) where T : class, IEntityWithId
    {
        var entity = context.Set<T>().SingleOrDefault(t => t.Id == id);
        if (entity == null)
            throw new EntityNotFoundException<T>(id);
        return entity;
    }

    public static async Task<T> GetOrDieAsync<T>(this DbContext context, int id) where T : class, IEntityWithId
    {
        var entity = await context.Set<T>().SingleOrDefaultAsync(t => t.Id == id);
        if (entity == null)
            throw new EntityNotFoundException<T>(id);
        return entity;
    }

    public static async Task<T> GetOrDieAsync<T>(this DbContext context, Expression<Func<T, bool>> predicate)
        where T : class
    {
        var entity = await context.Set<T>().SingleOrDefaultAsync(predicate);
        if (entity == null)
            throw new EntityNotFoundException<T>();
        return entity;
    }
}