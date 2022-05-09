// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Tests;

[TestClass]
public class ExceptionTests
{
    [TestMethod]
    public void TypeNotRegisteredException_Throw_SingleTypeProducesExpectedMessage()
    {
        Action throw_exception = () => throw new TypeNotRegisteredException(typeof(TestableService));
        throw_exception.Should().Throw<TypeNotRegisteredException>().WithMessage("InstanceType TestableService is not registered");
    }

    [TestMethod]
    public void TypeNotRegisteredException_Throw_MultipleTypesProducesExpectedMessage()
    {
        Action throw_exception = () => throw new TypeNotRegisteredException(new[] {typeof(TestableService), typeof(UnrelatedService)});
        throw_exception.Should().Throw<TypeNotRegisteredException>().WithMessage("InstanceType TestableService,UnrelatedService is not registered");
    }
}
