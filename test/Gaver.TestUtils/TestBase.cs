using System;
using LightInject;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Gaver.Data;

namespace Gaver.TestUtils
{
    public abstract class TestBase
    {
        protected readonly IServiceContainer Container;

        protected TestBase()
        {
            Container = new ServiceContainer(new ContainerOptions {
                EnableVariance = false,
                EnablePropertyInjection = false
            });
            Container.RegisterFallback((type, name) => true, request =>
            Substitute.For(new[] { request.ServiceType }, null), new PerContainerLifetime());
        }
    }

    public abstract class TestBase<TSut> : TestBase where TSut : class
    {
        private readonly Lazy<TSut> _testSubject;
        protected TSut TestSubject => _testSubject.Value;

        protected TestBase()
        {
            _testSubject = new Lazy<TSut>(() => Container.Create<TSut>());
        }
    }

    public abstract class DbTestBase<TSut> : TestBase<TSut> where TSut : class
    {
        protected DbTestBase()
        {
            var options = new DbContextOptionsBuilder<GaverContext>()
                .UseInMemoryDatabase().Options;
            Container.RegisterInstance<DbContextOptions<GaverContext>>(options);
        }
    }
}
