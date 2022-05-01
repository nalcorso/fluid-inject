// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableService : ITestableService
{
    public Guid ServiceId { get; } = Guid.NewGuid();
}
