// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject;

internal class TypeDescriptor
{
    public Type? InterfaceType { get; init; }
    public Type InstanceType { get; init; }
    public object? Instance { get; set; }

    public TypeLifetime Lifetime { get; init; }

    //public bool IsConstructable => false;

    public TypeDescriptor(object instance, TypeLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        if (instance.GetType().IsValueType)
            throw new ArgumentException("Cannot create a type descriptor for a value type");

        if (instance.GetType().FullName!.StartsWith("System."))
            throw new ArgumentException("Cannot create a type descriptor for a system type");

        if (lifetime == TypeLifetime.Transient)
            throw new ArgumentException("Transient instances cannot be constructed");

        InstanceType = instance.GetType();
        Instance = instance;
        Lifetime = lifetime;
    }

    public TypeDescriptor(Type interface_type, object instance, TypeLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));
        ArgumentNullException.ThrowIfNull(interface_type, nameof(interface_type));

        if (instance.GetType().IsValueType)
            throw new ArgumentException("Cannot create a type descriptor for a value type");

        if (instance.GetType().FullName!.StartsWith("System."))
            throw new ArgumentException("Cannot create a type descriptor for a system type");

        if (!interface_type.IsInterface)
            throw new ArgumentException("Type must be an interface");

        if (!instance.GetType().GetInterfaces().Contains(interface_type))
            throw new ArgumentException("The instance type must implement the interface type");

        if (lifetime == TypeLifetime.Transient)
            throw new ArgumentException("Transient instances cannot be constructed");

        InterfaceType = interface_type;
        InstanceType = instance.GetType();
        Instance = instance;
        Lifetime = lifetime;
    }

    public TypeDescriptor(Type instance_type, TypeLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(instance_type, nameof(instance_type));

        if (instance_type.FullName!.StartsWith("System."))
            throw new ArgumentException("Cannot create a type descriptor for a system type");

        if (instance_type.IsValueType)
            throw new ArgumentException("Cannot create a type descriptor for a value type");

        if (!instance_type.IsClass)
            throw new ArgumentException("Type must be a class");

        InstanceType = instance_type;
        Lifetime = lifetime;
    }

    public TypeDescriptor(Type interface_type, Type instance_type, TypeLifetime lifetime)
    {
        ArgumentNullException.ThrowIfNull(interface_type, nameof(interface_type));
        ArgumentNullException.ThrowIfNull(instance_type, nameof(instance_type));

        if (instance_type.FullName!.StartsWith("System."))
            throw new ArgumentException("Cannot create a type descriptor for a system type");

        if (!interface_type.IsInterface)
            throw new ArgumentException("The Interface type must be an interface");

        if (!instance_type.GetInterfaces().Contains(interface_type))
            throw new ArgumentException("The instance type must implement the interface type");

        InterfaceType = interface_type;
        InstanceType = instance_type;
        Lifetime = lifetime;
    }
}
