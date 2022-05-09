// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithFactory
{
    public delegate TestableServiceWithFactory Factory(int index, string name);

    public int Index { get; set; }
    public string Name { get; set; }

    public TestableServiceWithFactory(int index, string name)
    {
        Index = index;
        Name = name;
    }
}
