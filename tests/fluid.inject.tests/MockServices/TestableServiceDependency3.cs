// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceDependency3 : ITestableServiceDependency3
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}
