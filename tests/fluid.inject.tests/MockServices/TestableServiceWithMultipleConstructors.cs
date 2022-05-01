// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithMultipleConstructors
{
    private readonly ITestableServiceDependency1? _dependency1;
    private readonly ITestableServiceDependency2? _dependency2;
    private readonly ITestableServiceDependency3? _dependency3;

    public bool HasDependency1 => _dependency1 != null;
    public bool HasDependency2 => _dependency2 != null;
    public bool HasDependency3 => _dependency3 != null;

    public TestableServiceWithMultipleConstructors() {}

    public TestableServiceWithMultipleConstructors(ITestableServiceDependency1 dependency1)
    {
        _dependency1 = dependency1;
    }

    public TestableServiceWithMultipleConstructors(ITestableServiceDependency1 dependency1, ITestableServiceDependency2 dependency2)
    {
        _dependency1 = dependency1;
        _dependency2 = dependency2;
    }

    public TestableServiceWithMultipleConstructors(ITestableServiceDependency1 dependency1, ITestableServiceDependency2 dependency2, ITestableServiceDependency3 dependency3)
    {
        _dependency1 = dependency1;
        _dependency2 = dependency2;
        _dependency3 = dependency3;
    }
}
