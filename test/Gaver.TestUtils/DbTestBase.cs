using System;
using AutoMapper;
using Gaver.Data;
using LightInject;
using Microsoft.EntityFrameworkCore;

namespace Gaver.TestUtils
{
    public abstract class DbTestBase<TSut> : DbTestBase where TSut : class
    {
        private readonly Lazy<TSut> testSubjectLazy;

        protected DbTestBase()
        {
            testSubjectLazy = new Lazy<TSut>(() => Container.Create<TSut>());
            Container.RegisterAssembly(typeof(TSut).Assembly, (service, implementation) => service == typeof(Profile));
        }

        protected TSut TestSubject => testSubjectLazy.Value;
    }

    public abstract class DbTestBase : TestBase
    {
        protected DbTestBase()
        {
            var options = new DbContextOptionsBuilder<GaverContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            Container.RegisterInstance(options);
            Container.Register<GaverContext>(new PerContainerLifetime());
        }

        protected GaverContext Context => Get<GaverContext>();
    }
}
