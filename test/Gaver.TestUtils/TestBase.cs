using System;
using LightInject;
using NSubstitute;

namespace Gaver.TestUtils;

public abstract class TestBase {
    protected readonly IServiceContainer Container;

    protected TestBase() {
        Container = new ServiceContainer(new ContainerOptions {
            EnableVariance = false,
            EnablePropertyInjection = false
        });
        Container.RegisterFallback((type, name) => true, request =>
            Substitute.For([request.ServiceType], null), new PerContainerLifetime());
    }

    protected T Get<T>() {
        return Container.GetInstance<T>();
    }
}

public abstract class TestBase<TSut> : TestBase where TSut : class {
    private readonly Lazy<TSut> testSubjectLazy;

    protected TestBase() {
        testSubjectLazy = new Lazy<TSut>(() => Container.Create<TSut>());
    }

    protected TSut TestSubject => testSubjectLazy.Value;
}
