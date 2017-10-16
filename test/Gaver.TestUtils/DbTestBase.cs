using System;
using Microsoft.EntityFrameworkCore;
using Gaver.Data;

namespace Gaver.TestUtils
{
    public abstract class DbTestBase<TSut> : TestBase<TSut> where TSut : class
    {
        protected DbTestBase()
        {
            var options = new DbContextOptionsBuilder<GaverContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Container.RegisterInstance<DbContextOptions<GaverContext>>(options);
        }
    }
}
