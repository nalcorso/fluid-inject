// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithFactoryDependency
{
    public readonly TestableServiceWithFactory.Factory Factory;

    public TestableServiceWithFactoryDependency(TestableServiceWithFactory.Factory testable_service_factory)
    {
        Factory = testable_service_factory;
    }
}
