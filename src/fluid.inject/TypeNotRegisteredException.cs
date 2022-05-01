// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject;

internal class TypeNotRegisteredException : Exception
{
    public TypeNotRegisteredException(Type type) : base($"InstanceType {type.Name} is not registered") {}
    public TypeNotRegisteredException(IEnumerable<Type> types) : base($"InstanceType {string.Join(",", types.Select(t => t.Name))} is not registered") {}
}
