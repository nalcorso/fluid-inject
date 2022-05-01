// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceDependency1 : ITestableServiceDependency1
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}
