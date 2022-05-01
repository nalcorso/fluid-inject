// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithImplicitDependencies
{
    public IContainer Container { get; }

    public TestableServiceWithImplicitDependencies(IContainer container)
    {
        Container = container;
    }
}
