// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject;

//FIXME - Use a single monolithic interface until we are happy with the API. Ultimately we want to migrate to a state machine (of interfaces).
public interface ITypeDescriptor
{
    Type? ConcreteType { get; }
    object? Instance { get; }
    Type? InterfaceType { get; }
    Lifetime? Lifetime { get; }
    string? Name { get; }

    ITypeDescriptor As<TInterface>();
    ITypeDescriptor AsSingleton();
    ITypeDescriptor AsTransient();
    ITypeDescriptor WithName(string name);
}

public class TypeDescriptor : ITypeDescriptor
{
    public Type? ConcreteType { get; init; }
    public object? Instance { get; private set; }
    public Type? InterfaceType { get; private set; }
    public Lifetime? Lifetime { get; private set; }
    public string? Name { get; private set; }

    public TypeDescriptor(Type concrete_type)
    {
        ArgumentNullException.ThrowIfNull(concrete_type, nameof(concrete_type));

        if (!concrete_type.IsClass)
            throw new ArgumentException("Type must be a class");

        if (concrete_type.IsAbstract)
            throw new ArgumentException("Type may not be abstract");

        if (concrete_type.GetMethods().Any(m => m.Name == "<Clone>$"))
            throw new ArgumentException("Type may not be a record");

        if (concrete_type.FullName!.StartsWith("System."))
            throw new ArgumentException("Type may not be a system type");

        ConcreteType = concrete_type;
    }

    public TypeDescriptor(object instance)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        var concrete_type = instance.GetType();

        if (!concrete_type.IsClass)
            throw new ArgumentException("Type must be a class");

        if (concrete_type.IsAbstract)
            throw new ArgumentException("Type may not be abstract");

        if (concrete_type.GetMethods().Any(m => m.Name == "<Clone>$"))
            throw new ArgumentException("Type may not be a record");

        if (concrete_type.FullName!.StartsWith("System."))
            throw new ArgumentException("Type may not be a system type");

        ConcreteType = concrete_type;
        Instance = instance;
        Lifetime = Inject.Lifetime.Singleton;
    }

    public ITypeDescriptor As<TInterface>()
    {
        if (InterfaceType != null)
            throw new InvalidOperationException("Type already has an interface");

        if (!typeof(TInterface).IsInterface)
            throw new ArgumentException("Type must be an interface");

        if (!ConcreteType!.GetInterfaces().Contains(typeof(TInterface)))
            throw new ArgumentException("The instance type must implement this interface");

        InterfaceType = typeof(TInterface);

        return this;
    }

    public ITypeDescriptor AsSingleton()
    {
        if (Lifetime != null)
            throw new InvalidOperationException("Type already has a lifetime");

        Lifetime = Inject.Lifetime.Singleton;

        return this;
    }

    public ITypeDescriptor AsTransient()
    {
        if (Lifetime != null)
            throw new InvalidOperationException("Type already has a lifetime");

        if (Instance != null)
            throw new InvalidOperationException("Transient types may not have an instance");

        Lifetime = Inject.Lifetime.Transient;

        return this;
    }

    public ITypeDescriptor WithName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name may not be null or empty");

        if (Name != null)
            throw new InvalidOperationException("Type already has a name");

        Name = name;

        return this;
    }

    public ITypeDescriptor WithInstance(object instance)
    {
        if (Instance != null)
            throw new InvalidOperationException("Type already has an instance");

        if (Lifetime == Inject.Lifetime.Transient)
            throw new InvalidOperationException("Transient types may not have an instance");

        if (ConcreteType != instance.GetType())
            throw new ArgumentException("Instance must be of the same type as the concrete type");

        Instance = instance;
        Lifetime = Inject.Lifetime.Singleton;

        return this;
    }
}
