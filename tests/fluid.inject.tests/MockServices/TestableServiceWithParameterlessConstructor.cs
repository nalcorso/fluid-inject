// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests.MockServices;

public class TestableServiceWithParameterlessConstructor
{
    private IUnrelatedService _unrelated_service;

    public TestableServiceWithParameterlessConstructor()
    {
        _unrelated_service = new UnrelatedService();
    }
}
