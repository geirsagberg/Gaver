using System;
using System.Data.Common;
using AutoMapper;
using Gaver.Data;
using LightInject;
using Microsoft.Data.Sqlite;
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

    public abstract class DbTestBase : TestBase, IDisposable
    {
        private readonly DbConnection connection;

        protected DbTestBase()
        {
            connection = CreateInMemoryDatabase();
            var options = new DbContextOptionsBuilder<GaverContext>()
                .UseSqlite(CreateInMemoryDatabase())
                .Options;
            Container.RegisterInstance(options);
            Container.Register<GaverContext>(new PerContainerLifetime());
            using var context = Container.Create<GaverContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        protected GaverContext Context => Get<GaverContext>();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            connection.Dispose();
        }
    }
}
