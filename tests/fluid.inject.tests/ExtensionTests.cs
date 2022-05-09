// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests;

[TestClass]
public class ExtensionTests
{
    [TestMethod]
    public void TypeExtensions_IsDelegate_ReturnsTrueForDelegateTypes()
    {
        typeof(Action).IsDelegate().Should().BeTrue();
        typeof(Func<int>).IsDelegate().Should().BeTrue();
        typeof(Func<int, int>).IsDelegate().Should().BeTrue();
        typeof(TestableServiceWithFactory.Factory).IsDelegate().Should().BeTrue();
    }

    [TestMethod]
    public void TypeExtensions_IsDelegate_ReturnsFalseForNonDelegateTypes()
    {
        typeof(int).IsDelegate().Should().BeFalse();
        typeof(TestableServiceWithFactory).IsDelegate().Should().BeFalse();
    }
}
