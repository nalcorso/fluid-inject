// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithInvalidDependencies
{
    public int Dependency1 { get; }
    public string Dependency2 { get; }

    public TestableServiceWithInvalidDependencies(int dependency1, string dependency2)
    {
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }
}
