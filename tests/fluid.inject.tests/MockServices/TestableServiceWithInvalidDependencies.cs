// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithInvalidDependencies
{
    private readonly int _dependency1;
    private readonly string _dependency2;

    public TestableServiceWithInvalidDependencies(int dependency1, string dependency2)
    {
        _dependency1 = dependency1;
        _dependency2 = dependency2;
    }
}
