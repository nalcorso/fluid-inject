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
    public void Container_Creation_ShouldAllowProceduralInitialisation()
    {
        var container = new Container();
        container.AddSingleton<TestableService>();

        container.Has(typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_Creation_ShouldAllowFluentInitialisation()
    {
        var container = new Container().AddSingleton<TestableService>();

        container.Has(typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_AddSingleton_NullInstanceShouldThrow()
    {
        var container = new Container();

        container.Invoking(c => c.AddSingleton<ITestableService>(null!)).Should().Throw<ArgumentNullException>();
        container.Invoking(c => c.AddSingleton<TestableService>(null!)).Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void Container_AddSingleton_InterfaceWithoutInstanceShouldThrow()
    {
        var container = new Container();

        container.Invoking(c => c.AddSingleton<ITestableService>()).Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Container_AddSingleton_ShouldRegisterSingletonFromInstance()
    {
        var container = new Container().AddSingleton(new TestableService());

        container.Has(typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_AddSingleton_ShouldRegisterSingletonFromInstanceWithInterface()
    {
        var container = new Container().AddSingleton<ITestableService>(new TestableService());

        container.Has(typeof(TestableService)).Should().BeTrue();
        container.Has(typeof(ITestableService)).Should().BeTrue();
        container.Has(typeof(ITestableService), typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_AddSingleton_ShouldRegisterSingletonFromType()
    {
        var container = new Container().AddSingleton<TestableService>();

        container.Has(typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_AddSingleton_ShouldRegisterSingletonFromTypeWithInterface()
    {
        var container = new Container().AddSingleton<ITestableService, TestableService>();

        container.Has(typeof(TestableService)).Should().BeTrue();
        container.Has(typeof(ITestableService)).Should().BeTrue();
        container.Has(typeof(ITestableService), typeof(TestableService)).Should().BeTrue();
    }

    [TestMethod]
    public void Container_AddSingleton_AddingInstanceOfExistingTypeShouldThrow()
    {
        var container = new Container().AddSingleton(new TestableService());

        container.Invoking(c => c.AddSingleton(new TestableService())).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<ITestableService>(new TestableService())).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<TestableService>()).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<ITestableService, TestableService>()).Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void Container_AddSingleton_AddingExistingTypeShouldThrow()
    {
        var container = new Container().AddSingleton<TestableService>();

        container.Invoking(c => c.AddSingleton(new TestableService())).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<TestableService>()).Should().Throw<ArgumentException>();
        container.Invoking(c => c.AddSingleton<ITestableService, TestableService>()).Should().Throw<ArgumentException>();
    }

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
    }

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

    [TestMethod]
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
}
