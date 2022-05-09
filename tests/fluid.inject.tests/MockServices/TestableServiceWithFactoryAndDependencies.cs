// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithFactoryAndDependencies
{
    public delegate TestableServiceWithFactoryAndDependencies Factory(int index, string name);

    public int Index { get; set; }
    public string Name { get; set; }
    public TestableServiceDependency1 Dependency1 { get; }
    public TestableServiceDependency2 Dependency2 { get; }

    public TestableServiceWithFactoryAndDependencies(int index, string name, TestableServiceDependency1 dependency1, TestableServiceDependency2 dependency2)
    {
        Index = index;
        Name = name;
        Dependency1 = dependency1;
        Dependency2 = dependency2;
    }
}
