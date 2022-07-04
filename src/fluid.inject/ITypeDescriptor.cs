// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject;

// FIXME - Use a single monolithic interface until we are happy with the API. Ultimately we want to migrate to a state machine (of interfaces).
public interface ITypeDescriptor
{
    Type? ConcreteType { get; }
    object? Instance { get; }
    Type? InterfaceType { get; }
    Lifetime? Lifetime { get; }
    string? Name { get; }

    ITypeDescriptor As<TInterface>();
    ITypeDescriptor As(Type type);
    ITypeDescriptor AsSingleton();
    ITypeDescriptor AsTransient();
    ITypeDescriptor WithName(string name);
    ITypeDescriptor WithInstance(object instance);
    
}
