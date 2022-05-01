// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceDependency2 : ITestableServiceDependency2
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}
