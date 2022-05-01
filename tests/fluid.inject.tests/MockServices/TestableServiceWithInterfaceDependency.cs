// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithInterfaceDependency
{
    private readonly ITestableServiceDependency1 _dependency1;

    public Guid ServiceId => _dependency1.ServiceId;

    public TestableServiceWithInterfaceDependency(ITestableServiceDependency1 dependency1)
    {
        _dependency1 = dependency1;
    }
}
