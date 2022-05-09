// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithInvalidFactory
{
    public delegate TestableServiceWithInvalidFactory Factory(string name, int index);

    public int Index { get; set; }
    public string Name { get; set; }

    public TestableServiceWithInvalidFactory(int index, string name)
    {
        Index = index;
        Name = name;
    }
}
