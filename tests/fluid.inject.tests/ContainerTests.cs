// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests;

[TestClass]
public class ContainerTests
{
    [TestMethod]
    public void Container_Construction_ShouldSucceed()
    {
        var container = new Container();
        container.Should().NotBeNull();
    }

    [TestMethod]
    public void ContainerTests_Add_ReturnsMutableInstance()
    {
        var container = new Container();

        var descriptor = container.Add<TestableService>();
        container.Types.Count().Should().Be(1);
        descriptor.Should().BeSameAs(container.Types.First());
    }

    [TestMethod]
    public void ContainerTests_Add_ChainedConfigurationReturnsSameMutableInstance()
    {
        var container = new Container();

        var descriptor1 = container.Add<TestableService>();
        descriptor1.Should().BeSameAs(container.Types.First());

        var descriptor2 = descriptor1.As<ITestableService>();
        descriptor2.Should().BeSameAs(descriptor1);

        var descriptor3 = descriptor2.AsTransient();
        descriptor3.Should().BeSameAs(descriptor2);

        var descriptor4 = descriptor3.WithName("ServiceName");
        descriptor4.Should().BeSameAs(descriptor3);
    }

    [TestMethod]
    public void ContainerTests_Add_ChainedConfigurationShouldReturnExpected()
    {
        var container = new Container();

        _ = container.Add<TestableService>().As<ITestableService>().AsTransient().WithName("ServiceName");

        container.Types.Count().Should().Be(1);
        container.Types.First().ConcreteType.Should().Be(typeof(TestableService));
        container.Types.First().InterfaceType.Should().Be(typeof(ITestableService));
        container.Types.First().Lifetime.Should().Be(Lifetime.Transient);
        container.Types.First().Name.Should().Be("ServiceName");
    }

    [TestMethod]
    public void ContainerTests_Add_WithInstanceShouldReturnExpected()
    {
        var container = new Container();
        var service = new TestableService();

        _ = container.Add(service).As<ITestableService>().WithName("ServiceName");

        container.Types.Count().Should().Be(1);
        container.Types.First().ConcreteType.Should().Be(typeof(TestableService));
        container.Types.First().InterfaceType.Should().Be(typeof(ITestableService));
        container.Types.First().Instance.Should().BeSameAs(service);
        container.Types.First().Lifetime.Should().Be(Lifetime.Singleton);
        container.Types.First().Name.Should().Be("ServiceName");
    }

    [TestMethod]
    public void ContainerTests_Get_TransientServiceShouldReturnNewInstance()
    {
        var container = new Container();
        _ = container.Add<TestableService>().As<ITestableService>().AsTransient();

        var service1 = container.Get<ITestableService>();
        var service2 = container.Get<ITestableService>();

        service1.Should().NotBeSameAs(service2);
    }

    [TestMethod]
    public void ContainerTests_Get_SingletonServiceShouldReturnSameInstance()
    {
        var container = new Container();
        _ = container.Add<TestableService>().As<ITestableService>().AsSingleton();

        var service1 = container.Get<ITestableService>();
        var service2 = container.Get<ITestableService>();

        service1.Should().BeSameAs(service2);
    }

    [TestMethod]
    public void ContainerTests_Get_ServiceWithDependenciesShouldReturnExpected()
    {
        var container = new Container();

        _ = container.Add<TestableServiceDependency1>().As<ITestableServiceDependency1>().AsSingleton();
        _ = container.Add<TestableServiceDependency2>().As<ITestableServiceDependency2>().AsSingleton();
        _ = container.Add<TestableServiceWithTypedDependency>().AsSingleton();
        _ = container.Add<TestableServiceWithInterfaceDependency>().AsTransient();

        var service1 = container.Get<TestableServiceWithTypedDependency>();

        service1.Dependency1.Should().BeOfType<TestableServiceDependency1>();

        var service2 = container.Get<TestableServiceWithInterfaceDependency>();

        service2.Dependency1.Should().BeOfType<TestableServiceDependency1>();

        service1.Dependency1.Should().BeSameAs(service2.Dependency1);
    }

    [TestMethod]
    public void ContainerTests_Get_ServiceWithImplicitDependenciesShouldReturnExpected()
    {
        var container = new Container();

        _ = container.Add<TestableServiceWithImplicitDependencies>().AsTransient();

        var service = container.Get<TestableServiceWithImplicitDependencies>();

        service.Should().NotBeNull();
        service.Container.Should().BeOfType<Container>();
        service.Container.Should().BeSameAs(container);
    }

    [TestMethod]
    public void ContainerTests_Get_ServiceWithFactoryShouldReturnExpected()
    {
        var container = new Container();

        _ = container.Add<TestableServiceWithFactory>().AsTransient();

        var service = container.Get<TestableServiceWithFactory.Factory>();
        service.Should().NotBeNull();
        service.GetType().Should().Be(typeof(TestableServiceWithFactory.Factory));
    }

    [TestMethod]
    public void ContainerTests_Get_ServiceWithFactoryDependencyShouldReturnExpected()
    {
        var container = new Container();

        _ = container.Add<TestableServiceWithFactoryDependency>().AsTransient();
        _ = container.Add<TestableServiceWithFactory>().AsTransient();

        var service = container.Get<TestableServiceWithFactoryDependency>();
        service.Should().NotBeNull();
        service.Factory.Should().NotBeNull();
        service.Factory.Should().BeOfType<TestableServiceWithFactory.Factory>();
    }

    [TestMethod]
    public void ContainerTests_Get_ServiceWithFactoryAndDependenciesShouldReturnExpected()
    {
        var container = new Container();

        _ = container.Add<TestableServiceDependency1>().AsTransient();
        _ = container.Add<TestableServiceDependency2>().AsTransient();
        _ = container.Add<TestableServiceWithFactoryAndDependencies>().AsTransient();

        var service = container.Get<TestableServiceWithFactoryAndDependencies.Factory>();
        service.Should().NotBeNull();

        var instance = service(42, "Foo");

        instance.Should().NotBeNull();
        instance.Dependency1.Should().BeOfType<TestableServiceDependency1>();
        instance.Dependency2.Should().BeOfType<TestableServiceDependency2>();
        instance.Dependency1.Should().NotBeSameAs(instance.Dependency2);
        instance.Index.Should().Be(42);
        instance.Name.Should().Be("Foo");
    }

    [TestMethod]
    public void ContainerTests_Get_ServiceWithMissingDependencyShouldThrow()
    {
        var container = new Container();

        _ = container.Add<TestableServiceWithTypedDependency>().AsTransient();

        Action get_action = () => container.Get<TestableServiceWithTypedDependency>();
        get_action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void ContainerTests_Get_ServiceWithInvalidDependencyShouldThrow()
    {
        var container = new Container();

        _ = container.Add<TestableServiceWithInvalidDependencies>().AsTransient();

        Action get_action = () => container.Get<TestableServiceWithInvalidDependencies>();
        get_action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void ContainerTests_Get_FactoryWithIncorrectSignatureShouldThrow()
    {
        var container = new Container();

        _ = container.Add<TestableServiceWithInvalidFactory>().AsTransient();

        Action get_action = () => container.Get<TestableServiceWithInvalidFactory>();
        get_action.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void ContainerTests_Get_InvalidTypesShouldThrow()
    {
        var container = new Container();

        Action get_action1 = () => container.Get<UnrelatedService>();
        get_action1.Should().Throw<ArgumentException>();

        Action get_action2 = () => container.Get<string>();
        get_action2.Should().Throw<ArgumentException>();
    }

#region Obsolete Test Methods
    /*




    [TestMethod]
    public void Container_AddSingleton_AddingExistingTypeWithInterfaceShouldThrow()
    {
        var container = new Container().AddSingleton<ITestableService, TestableService>();

        container.Invoking(c => c.AddSingleton(new TestableService())).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<TestableService>()).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<ITestableService, TestableService>()).Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Container_AddSingleton_AddingExistingInstanceShouldThrow()
    {
        var service_instance = new TestableService();

        var container = new Container().AddSingleton(service_instance);

        container.Invoking(c => c.AddSingleton(service_instance)).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<ITestableService>(service_instance)).Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Container_AddTransient_ShouldRegisterTransientFromType()
    {
        var container = new Container().AddTransient<TestableService>();

        container.Has(typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_AddTransient_ShouldRegisterTransientFromInterfaceAndType()
    {
        var container = new Container().AddTransient<ITestableService, TestableService>();

        container.Has(typeof(TestableService)).Should().BeTrue();
        container.Has(typeof(ITestableService)).Should().BeTrue();
        container.Has(typeof(ITestableService), typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_AddTransient_AddingExistingTypeShouldThrow()
    {
        var container = new Container().AddTransient<TestableService>();

        container.Invoking(c => c.AddTransient<TestableService>()).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddTransient<ITestableService, TestableService>()).Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Container_AddTransient_AddingExistingTypeWithInterfaceShouldThrow()
    {
        var container = new Container().AddTransient<ITestableService, TestableService>();

        container.Invoking(c => c.AddTransient<TestableService>()).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddTransient<ITestableService, TestableService>()).Should().Throw<ArgumentException>();
    }*/

    /*[TestMethod]
    public void Container_RegisterSingleton_ShouldRegisterSingletonWithName()
    {
        var container = new Container();
        container.RegisterSingleton<SingletonTestService>("test");
        var container = container.Build();
        container.Resolve<SingletonTestService>("test").Should().NotBeNull();
    }*/

    /*[TestMethod]
    public void Container_RegisterTransient_ShouldRegisterTransientWithName()
    {
        var container = new Container();
        container.RegisterTransient<TransientTestService>("test");
        var container = container.Build();
        container.Resolve<TransientTestService>("test").Should().NotBeNull();
    }*/

    /*[TestMethod]
    public void Container_Get_GetMissingTypeShouldThrow()
    {
        var container = new Container();
        container.Invoking(c => c.Get<ITestableService>()).Should().Throw<TypeNotRegisteredException>();
        container.Invoking(c => c.Get<TestableService>()).Should().Throw<TypeNotRegisteredException>();
    }

    [TestMethod]
    public void Container_Get_GetSingletonShouldReturnSameInstance()
    {
        var container = new Container().AddSingleton<ITestableService, TestableService>();

        container.Get<TestableService>().Should().NotBeNull();
        container.Get<ITestableService>().Should().NotBeNull();
        container.Get<TestableService>().Should().BeSameAs(container.Get<TestableService>());
        container.Get<ITestableService>().Should().BeSameAs(container.Get<TestableService>());
        container.Get<ITestableService>().Should().BeSameAs(container.Get<ITestableService>());
        container.Get<TestableService>().ServiceId.Should().Be(container.Get<TestableService>().ServiceId);
        container.Get<ITestableService>().ServiceId.Should().Be(container.Get<TestableService>().ServiceId);
        container.Get<ITestableService>().ServiceId.Should().Be(container.Get<ITestableService>().ServiceId);
    }

    [TestMethod]
    public void Container_Get_GetTransientShouldReturnNewInstance()
    {
        var container = new Container().AddTransient<ITestableService, TestableService>();

        container.Get<TestableService>().Should().NotBeNull();
        container.Get<ITestableService>().Should().NotBeNull();
        container.Get<TestableService>().Should().NotBeSameAs(container.Get<TestableService>());
        container.Get<ITestableService>().Should().NotBeSameAs(container.Get<TestableService>());
        container.Get<ITestableService>().Should().NotBeSameAs(container.Get<ITestableService>());
        container.Get<TestableService>().ServiceId.Should().NotBe(container.Get<TestableService>().ServiceId);
        container.Get<ITestableService>().ServiceId.Should().NotBe(container.Get<TestableService>().ServiceId);
        container.Get<ITestableService>().ServiceId.Should().NotBe(container.Get<ITestableService>().ServiceId);
    }

    [TestMethod]
    public void Container_Get_GetTypeShouldReturnInstance()
    {
        var container = new Container().AddSingleton<TestableService>();

        var service = container.Get<TestableService>();
        service.Should().NotBeNull();
    }

    [TestMethod]
    public void Container_Get_GetTypeWithTypedDependencyShouldReturnInstance()
    {
        var container = new Container().AddSingleton<TestableServiceWithTypedDependency>().AddSingleton<TestableServiceDependency1>();

        var service = container.Get<TestableServiceWithTypedDependency>();
        service.Should().NotBeNull();
    }

    [TestMethod]
    public void Container_Get_GetTypeWithInterfaceDependencyShouldReturnInstance()
    {
        var container = new Container().AddSingleton<TestableServiceWithInterfaceDependency>().AddSingleton<ITestableServiceDependency1, TestableServiceDependency1>();

        var service = container.Get<TestableServiceWithInterfaceDependency>();
        service.Should().NotBeNull();
    }

    [TestMethod]
    public void Container_Get_GetTypeWithImplicitDependenciesShouldReturnInstance()
    {
        var container = new Container().AddSingleton<TestableServiceWithImplicitDependencies>();

        var service = container.Get<TestableServiceWithImplicitDependencies>();
        service.Should().NotBeNull();
        service.Container.Should().NotBeNull();
        service.Container.Should().BeSameAs(container);
    }

    [TestMethod]
    public void Container_Get_GetInterfaceFromTypedServiceShouldThrow()
    {
        var container = new Container().AddSingleton<TestableService>();

        container.Invoking(c => c.Get<ITestableService>()).Should().Throw<TypeNotRegisteredException>();
    }

    [TestMethod]
    public void Container_Get_GetTypeWithMissingDependencyShouldThrow()
    {
        var container = new Container().AddSingleton<TestableServiceWithTypedDependency>().AddSingleton<TestableServiceWithInterfaceDependency>();

        container.Invoking(c => c.Get<TestableServiceWithTypedDependency>()).Should().Throw<ArgumentException>();
        container.Invoking(c => c.Get<TestableServiceWithInterfaceDependency>()).Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Container_Get_GetTypeWithMissingDependencyInterfaceShouldThrow()
    {
        var container = new Container().AddSingleton<TestableServiceWithInterfaceDependency>().AddSingleton<TestableServiceDependency1>();

        container.Invoking(c => c.Get<TestableServiceWithInterfaceDependency>()).Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Container_Get_GetTypeWithSingletonDependencyShouldReturnSameInstance()
    {
        var container = new Container().AddSingleton<TestableServiceWithTypedDependency>().AddSingleton<TestableServiceDependency1>();

        var service1 = container.Get<TestableServiceWithTypedDependency>();
        var service2 = container.Get<TestableServiceDependency1>();
        service1.ServiceId.Should().Be(service2.ServiceId);
    }
    */
#endregion
}
