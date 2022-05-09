// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithFactoryDependency
{
    private readonly TestableServiceWithFactory.Factory _testable_service_factory;

    public TestableServiceWithFactoryDependency(TestableServiceWithFactory.Factory testable_service_factory)
    {
        _testable_service_factory = testable_service_factory;
    }
}
