// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithTypedDependency
{
    public TestableServiceDependency1 Dependency1 { get; }
    public TestableServiceDependency2 Dependency2 { get; }

    public Guid ServiceId => Dependency1.ServiceId;

    public TestableServiceWithTypedDependency(TestableServiceDependency1 dependency1, TestableServiceDependency2 dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }
}
