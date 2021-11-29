using System;
using System.Data.Common;
using AutoMapper;
using Gaver.Data;
using LightInject;
using Microsoft.EntityFrameworkCore;

namespace Gaver.TestUtils;

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
        connection = DbUtils.CreateInMemoryDbConnection();
        var options = new DbContextOptionsBuilder<GaverContext>()
            .UseSqlite(DbUtils.CreateInMemoryDbConnection())
            .Options;
        Container.RegisterInstance(options);
        Container.Register<GaverContext>(new PerContainerLifetime());
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();
    }

    protected GaverContext Context => Get<GaverContext>();

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        connection.Dispose();
    }
}