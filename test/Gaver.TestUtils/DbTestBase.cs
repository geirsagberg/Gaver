using System;
using Gaver.Data;
using LightInject;
using Microsoft.EntityFrameworkCore;

namespace Gaver.TestUtils
{
    public abstract class DbTestBase<TSut> : TestBase<TSut> where TSut : class
    {
        protected DbTestBase()
        {
            var options = new DbContextOptionsBuilder<GaverContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Container.RegisterInstance(options);
            Container.Register<GaverContext>(new PerContainerLifetime());
        }

        public GaverContext Context => Get<GaverContext>();
    }
}