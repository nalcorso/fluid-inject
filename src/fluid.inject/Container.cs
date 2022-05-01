// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject;

public class Container : IContainer
{
    private readonly List<TypeDescriptor> _registry = new();

    public bool Has(Type type)
    {
        return _registry.Any(x => (x.InterfaceType == type) || (x.InstanceType == type));
    }

    public bool Has(Type interface_type, Type instance_type)
    {
        return _registry.Any(x => (x.InterfaceType == interface_type) || (x.InstanceType == instance_type));
    }

    public IContainer AddSingleton<T>() where T : class
    {
        if (Has(typeof(T)))
            throw new ArgumentException($"InstanceType {typeof(T)} is already registered.");

        _registry.Add(new TypeDescriptor(typeof(T), TypeLifetime.Singleton));

        return this;
    }

    public IContainer AddSingleton<I, T>() where I : class where T : class, I
    {
        if (Has(typeof(I), typeof(T)))
            throw new ArgumentException($"InterfaceType {typeof(I)} or InstanceType {typeof(T)} is already registered.");

        _registry.Add(new TypeDescriptor(typeof(I), typeof(T), TypeLifetime.Singleton));

        return this;
    }

    public IContainer AddTransient<T>() where T : class
    {
        if (Has(typeof(T)))
            throw new ArgumentException($"InstanceType {typeof(T)} is already registered.");

        _registry.Add(new TypeDescriptor(typeof(T), TypeLifetime.Transient));

        return this;
    }

    public IContainer AddTransient<I, T>() where I : class where T : class, I
    {
        if (Has(typeof(I), typeof(T)))
            throw new ArgumentException($"InterfaceType {typeof(I)} or InstanceType {typeof(T)} is already registered.");

        _registry.Add(new TypeDescriptor(typeof(I), typeof(T), TypeLifetime.Transient));

        return this;
    }

    public IContainer AddNamedSingleton<T>(string name) where T : class
    {
        throw new NotImplementedException();
    }

    public IContainer AddNamedSingleton<I, T>(string name) where I : class where T : class, I
    {
        throw new NotImplementedException();
    }

    public IContainer AddNamedSingleton<T>(string name, T instance) where T : class
    {
        throw new NotImplementedException();
    }

    public IContainer AddNamedTransient<T>(string name) where T : class
    {
        throw new NotImplementedException();
    }

    public IContainer AddNamedTransient<I, T>(string name) where I : class where T : class, I
    {
        throw new NotImplementedException();
    }

    public T Get<T>() where T : class
    {
        //FIXME - Interface Types Map to Concrete Types & Concrete Types Map Directly. The two should be separated - perhaps a dictionary<InterfaceType, ConcreteType>?

        var descriptor = _registry.FirstOrDefault(d => (d.InterfaceType == typeof(T)) || (d.InstanceType == typeof(T)));

        if (descriptor is null)
            throw new TypeNotRegisteredException(typeof(T));

        if (descriptor.Instance is not null)
            return (T)descriptor.Instance;

        return (T)InstantiateType(descriptor);
    }

    public IContainer AddSingleton<T>(T instance) where T : class
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        if (Has(instance.GetType()))
            throw new ArgumentException($"InstanceType {typeof(T)} is already registered.");

        if (typeof(T).IsInterface)
            _registry.Add(new TypeDescriptor(typeof(T), instance, TypeLifetime.Singleton));
        else
            _registry.Add(new TypeDescriptor(instance, TypeLifetime.Singleton));

        return this;
    }

    private bool CanInstantiateType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if ((type == typeof(Container)) || (type == typeof(IContainer)))
            return true;

        if (_registry.All(d => (d.InterfaceType != type) && (d.InstanceType != type)))
            return false;

        //FIXME - We need to test whether it is necessary to have this short circuit
        //if (type.GetConstructors().Any(c => c.GetParameters().Length == 0))
        //    return true;

        if (type.GetConstructors().Length == 0)
            return true;

        return type.GetConstructors().Any(c => c.GetParameters().All(p => CanInstantiateType(p.ParameterType)));
    }

    private object InstantiateType(TypeDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);

        if (!CanInstantiateType(descriptor.InstanceType))
        {
            if (descriptor.InstanceType.IsAbstract || descriptor.InstanceType.IsInterface || descriptor.InstanceType.IsValueType)
                throw new ArgumentException($"Type {descriptor.InstanceType.Name} is abstract, interface or value type");

            //FIXME - We want to generate some kind of dependency graph so that we can easily determine missing dependecies and accurately report them.
            //var missing_types = _registry.Where(d => d.InstanceType != descriptor.InstanceType).Select(d => d.InstanceType).ToList();
            //if (missing_types.Any())
            //    throw new TypeNotRegisteredException(missing_types);

            //FIXME - We want to know if a mismatch may have occurred when registering AddTransient<Dependency> when we wanted AddTransient<Interface, Dependency>

            throw new ArgumentException($"No constructor for {descriptor.InstanceType.Name} found that can be used");
        }

        //FIXME - Constructor selection will change to allow for more concrete tie breaking of constructor selection
        var constructor = descriptor.InstanceType.GetConstructors().Where(c => c.GetParameters().All(p => CanInstantiateType(p.ParameterType))).OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();

        if (constructor is null)
            throw new Exception("Not sure we should be here?!?!");

        object? result = null;

        var parameters = constructor.GetParameters();

        if (parameters.Length == 0)
        {
            result = Activator.CreateInstance(descriptor.InstanceType);
        }
        else
        {
            var args = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                var arg_descriptor = _registry.Find(d => (d.InstanceType == parameter.ParameterType) || (d.InterfaceType == parameter.ParameterType));

                if ((parameter.ParameterType == typeof(IContainer)) || (parameter.ParameterType == typeof(Container)))
                    args[i] = this;
                else if (arg_descriptor is null)
                    throw new TypeNotRegisteredException(parameter.ParameterType);
                else
                    args[i] = InstantiateType(arg_descriptor);
            }

            result = Activator.CreateInstance(descriptor.InstanceType, args);
        }

        if (result is null)
            throw new Exception($"Failed to create instance of type {descriptor.InstanceType}");

        if (descriptor.Lifetime == TypeLifetime.Singleton)
            descriptor.Instance = result;

        return result;
    }
}
