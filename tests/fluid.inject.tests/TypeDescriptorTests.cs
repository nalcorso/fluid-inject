// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests;

[TestClass]
public class TypeDescriptorTests
{
    [TestMethod]
    public void TypeDescriptor_Constructor_FromNullShouldThrow()
    {
        Action construction = () => new TypeDescriptor(null!);
        construction.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromInterfaceShouldThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(ITestableService));
        construction.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromAbstractShouldThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(AbstractTestableService));
        construction.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromSystemTypeShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(string));
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(typeof(int));
        construction2.Should().Throw<ArgumentException>();

        Action construction3 = () => new TypeDescriptor(typeof(List<string>));
        construction3.Should().Throw<ArgumentException>();

        Action construction4 = () => new TypeDescriptor(new string("foo"));
        construction4.Should().Throw<ArgumentException>();

        Action construction5 = () => new TypeDescriptor("foo");
        construction5.Should().Throw<ArgumentException>();

        Action construction6 = () => new TypeDescriptor(1);
        construction6.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromStructShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableStruct));
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(new TestableStruct());
        construction2.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromRecordShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableRecord));
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(new TestableRecord());
        construction2.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromEnumShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableEnum));
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(TestableEnum.Value1);
        construction2.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromValidTypeShouldNotThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableService));
        construction1.Should().NotThrow();

        Action construction2 = () => new TypeDescriptor(new TestableService());
        construction2.Should().NotThrow();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromValidTypeShouldReturnExpected()
    {
        var descriptor1 = new TypeDescriptor(typeof(TestableService));
        descriptor1.ConcreteType.Should().Be(typeof(TestableService));

        var service = new TestableService();
        var descriptor2 = new TypeDescriptor(service);
        descriptor2.ConcreteType.Should().Be(typeof(TestableService));
        descriptor2.Lifetime.Should().Be(Lifetime.Singleton);
        descriptor2.Instance.Should().BeSameAs(service);
    }

    [TestMethod]
    public void TypeDescriptor_As_ValidInterfaceShouldNotThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(TestableService)).As<ITestableService>();
        construction.Should().NotThrow();
    }

    [TestMethod]
    public void TypeDescriptor_As_ValidInterfaceShouldReturnExpected()
    {
        var descriptor = new TypeDescriptor(typeof(TestableService)).As<ITestableService>();
        descriptor.ConcreteType.Should().Be(typeof(TestableService));
        descriptor.InterfaceType.Should().Be(typeof(ITestableService));
    }

    [TestMethod]
    public void TypeDescriptor_As_SubsequentCallsShouldThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(TestableService)).As<ITestableService>().As<ITestableService>();
        construction.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TypeDescriptor_As_InvalidTypeShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableService)).As<UnrelatedService>();
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(typeof(TestableService)).As<string>();
        construction2.Should().Throw<ArgumentException>();

        Action construction3 = () => new TypeDescriptor(typeof(TestableService)).As<int>();
        construction3.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_As_InvalidInterfaceShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableService)).As<IUnrelatedService>();
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(typeof(TestableService)).As<IEnumerable<string>>();
        construction2.Should().Throw<ArgumentException>();

        Action construction3 = () => new TypeDescriptor(new TestableService()).As<IUnrelatedService>();
        construction3.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_AsSingleton_ValidCallShouldNotThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableService)).AsSingleton();
        construction1.Should().NotThrow();
    }

    [TestMethod]
    public void TypeDescriptor_AsSingleton_ValidCallShouldReturnExpected()
    {
        var descriptor = new TypeDescriptor(typeof(TestableService)).AsSingleton();
        descriptor.ConcreteType.Should().Be(typeof(TestableService));
        descriptor.Lifetime.Should().Be(Lifetime.Singleton);
    }

    [TestMethod]
    public void TypeDescriptor_AsSingleton_RedundantCallShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(new TestableService()).AsSingleton();
        construction1.Should().Throw<InvalidOperationException>();

        Action construction2 = () => new TypeDescriptor(typeof(TestableService)).AsSingleton().AsSingleton();
        construction2.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TypeDescriptor_AsTransient_ValidCallShouldNotThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableService)).AsTransient();
        construction1.Should().NotThrow();
    }

    [TestMethod]
    public void TypeDescriptor_AsTransient_ValidCallShouldReturnExpected()
    {
        var descriptor = new TypeDescriptor(typeof(TestableService)).AsTransient();
        descriptor.ConcreteType.Should().Be(typeof(TestableService));
        descriptor.Lifetime.Should().Be(Lifetime.Transient);
    }

    [TestMethod]
    public void TypeDescriptor_AsTransient_RedundantCallShouldThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(TestableService)).AsTransient().AsTransient();
        construction.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TypeDescriptor_AsTransient_InvalidCallShouldThrow()
    {
        Action construction = () => new TypeDescriptor(new TestableService()).AsTransient();
        construction.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TypeDescriptor_WithInstance_ValidCallShouldNotThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(TestableService)).WithInstance(new TestableService());
        construction.Should().NotThrow();
    }

    [TestMethod]
    public void TypeDescriptor_WithInstance_ValidCallShouldReturnExpected()
    {
        var instance = new TestableService();
        var descriptor = new TypeDescriptor(typeof(TestableService)).WithInstance(instance);
        descriptor.ConcreteType.Should().Be(typeof(TestableService));
        descriptor.Lifetime.Should().Be(Lifetime.Singleton);
        descriptor.Instance.Should().BeSameAs(instance);
    }

    [TestMethod]
    public void TypeDescriptor_WithInstance_RedundantCallShouldThrow()
    {
        Action construction = () => new TypeDescriptor(new TestableService()).WithInstance(new TestableService());
        construction.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TypeDescriptor_WithInstance_TypeMismatchShouldThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(TestableService)).WithInstance(new UnrelatedService());
        construction.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    public void TypeDescriptor_WithInstance_NullInstanceShouldThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(TestableService)).WithInstance(null!);
        construction.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TypeDescriptor_WithInstance_InvalidLifetimeShouldThrow()
    {
        // Note: This test will become redundant when we add support for the Interface State Machine.
        Action construction = () => new TypeDescriptor(typeof(TestableService)).AsTransient().WithInstance(new TestableService());
        construction.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void TypeDescriptor_WithName_ValidCallShouldNotThrow()
    {
        Action construction = () => new TypeDescriptor(typeof(TestableService)).WithName("TestableService");
        construction.Should().NotThrow();
    }

    [TestMethod]
    public void TypeDescriptor_WithName_ValidCallShouldReturnExpected()
    {
        var descriptor = new TypeDescriptor(typeof(TestableService)).WithName("TestableService");
        descriptor.ConcreteType.Should().Be(typeof(TestableService));
        descriptor.Name.Should().Be("TestableService");
    }

    [TestMethod]
    public void TypeDescriptor_WithName_InvalidCallShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(TestableService)).WithName(null!);
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(typeof(TestableService)).WithName("");
        construction2.Should().Throw<ArgumentException>();

        Action construction3 = () => new TypeDescriptor(typeof(TestableService)).WithName("TestableService").WithName("TestableService");
        construction3.Should().Throw<InvalidOperationException>();
    }
}
