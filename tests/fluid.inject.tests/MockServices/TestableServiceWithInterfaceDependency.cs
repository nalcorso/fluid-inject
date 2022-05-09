// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithInterfaceDependency
{
    public readonly ITestableServiceDependency1 Dependency1;

    public Guid ServiceId => Dependency1.ServiceId;

    public TestableServiceWithInterfaceDependency(ITestableServiceDependency1 dependency1)
    {
        Dependency1 = dependency1;
    }
}
