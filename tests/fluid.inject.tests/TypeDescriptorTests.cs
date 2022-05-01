// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests;

[TestClass]
public class TypeDescriptorTests
{
    [TestMethod]
    public void TypeDescriptor_Constructor_FromInvalidTypeShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor("ValueType", TypeLifetime.Singleton);
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(typeof(string), TypeLifetime.Singleton);
        construction2.Should().Throw<ArgumentException>();

        Action construction3 = () => new TypeDescriptor(typeof(string), "ValueType", TypeLifetime.Singleton);
        construction3.Should().Throw<ArgumentException>();

        Action construction4 = () => new TypeDescriptor(null!, TypeLifetime.Singleton);
        construction4.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromInstanceReturnsExpectedTypeDescriptor()
    {
        var descriptor = new TypeDescriptor(new TestableService(), TypeLifetime.Singleton);

        descriptor.InstanceType.Should().Be(typeof(TestableService));
        descriptor.Lifetime.Should().Be(TypeLifetime.Singleton);
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromInstanceAndInterfaceReturnsExpectedTypeDescriptor()
    {
        var descriptor = new TypeDescriptor(typeof(ITestableService), new TestableService(), TypeLifetime.Singleton);

        descriptor.InterfaceType.Should().Be(typeof(ITestableService));
        descriptor.InstanceType.Should().Be(typeof(TestableService));
        descriptor.Lifetime.Should().Be(TypeLifetime.Singleton);
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromTypeReturnsExpectedTypeDescriptor()
    {
        var descriptor = new TypeDescriptor(typeof(TestableService), TypeLifetime.Singleton);

        descriptor.InstanceType.Should().Be(typeof(TestableService));
        descriptor.Lifetime.Should().Be(TypeLifetime.Singleton);
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromTypeAndInterfaceReturnsExpectedTypeDescriptor()
    {
        var descriptor = new TypeDescriptor(typeof(ITestableService), typeof(TestableService), TypeLifetime.Singleton);

        descriptor.InterfaceType.Should().Be(typeof(ITestableService));
        descriptor.InstanceType.Should().Be(typeof(TestableService));
        descriptor.Lifetime.Should().Be(TypeLifetime.Singleton);
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromInstanceWithTransientLifetimeShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(new TestableService(), TypeLifetime.Transient);
        construction1.Should().Throw<ArgumentException>().WithMessage("Transient instances cannot be constructed");

        Action construction2 = () => new TypeDescriptor(typeof(ITestableService), new TestableService(), TypeLifetime.Transient);
        construction2.Should().Throw<ArgumentException>().WithMessage("Transient instances cannot be constructed");
    }

    [TestMethod]
    public void TypeDescriptor_Constructor_FromTypeWithIncorrectInterfaceShouldThrow()
    {
        Action construction1 = () => new TypeDescriptor(typeof(IUnrelatedService), new TestableService(), TypeLifetime.Singleton);
        construction1.Should().Throw<ArgumentException>();

        Action construction2 = () => new TypeDescriptor(typeof(IUnrelatedService), typeof(TestableService), TypeLifetime.Singleton);
        construction2.Should().Throw<ArgumentException>();
    }
}
