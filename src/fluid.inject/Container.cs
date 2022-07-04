// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

using System.Linq.Expressions;
using System.Reflection;
using Fluid.Inject.Exceptions;
using Fluid.Inject.Extensions;

namespace Fluid.Inject;

public class Container : IContainer
{
    private readonly List<TypeDescriptor> _types = new();

    internal IEnumerable<TypeDescriptor> Types => _types;

    public ITypeDescriptor Add<T>()
    {
        var result = new TypeDescriptor(typeof(T));

        _types.Add(result);

        return result;
    }

    public ITypeDescriptor Add(object instance)
    {
        var result = new TypeDescriptor(instance);

        _types.Add(result);

        return result;
    }

    public ITypeDescriptor Add(Type type)
    {
        var result = new TypeDescriptor(type);

        _types.Add(result);

        return result;
    }

    public T Get<T>()
    {
        return (T)Resolve(typeof(T), null);
    }

    public T Get<T>(string name)
    {
        return (T)Resolve(typeof(T), name);
    }

    private bool CanResolveType(Type type)
    {
        if ((type == typeof(IContainer)) || (type == typeof(Container)))
            return true;

        if (type.IsGenericType && !type.IsGenericTypeDefinition)
            return CanResolveType(type.GetGenericTypeDefinition());

        if (type.IsDelegate() && type.DeclaringType is not null)
            return CanResolveType(type.DeclaringType);

        if (!_types.Any(t => (t.ConcreteType == type) || (t.InterfaceType == type)))
            return false;

        if (type.GetConstructors().Length == 0)
            return true;

        // FIXME - Check and prevent circular dependencies
        return type.GetConstructors().Select(CanResolveParametersForConstructor).Any();
    }

    private bool CanResolveParametersForConstructor(ConstructorInfo constructor_info)
    {
        if (constructor_info.GetParameters().Length == 0)
            return true;

        // FIXME - Ultimately change this to a simpler LINQ statement All. Long form is for debugging.
        foreach (var parameter_info in constructor_info.GetParameters())
        {
            if (!CanResolveType(parameter_info.ParameterType))
                return false;
        }

        return true;
    }

    private bool CanResolveParametersForConstructor(ConstructorInfo constructor_info, IReadOnlyCollection<ParameterInfo> parameters)
    {
        if (constructor_info.GetParameters().Length < parameters.Count)
            return false;

        foreach (var parameter in parameters)
        {
            if (constructor_info.GetParameters()[parameter.Position].ParameterType != parameter.ParameterType)
                return false;

            if (constructor_info.GetParameters()[parameter.Position].Name != parameter.Name)
                return false;
        }

        foreach (var parameter_info in constructor_info.GetParameters().Skip(parameters.Count))
        {
            if (!CanResolveType(parameter_info.ParameterType))
                return false;
        }

        return true;
    }

    private object BuildFactoryExpression(Type type)
    {
        // Dynamically build an Expression Tree for the following factory
        // (param1, param2) => new ConcreteType(param1, param2, service1)

        var delegate_type = type;

        var method = delegate_type.GetMethod("Invoke");

        if (method is null)
            throw new InvalidOperationException("Delegate type must have an Invoke method");

        var delegate_parameters = new List<ParameterExpression>();

        foreach (var param in method.GetParameters())
        {
            delegate_parameters.Add(Expression.Parameter(param.ParameterType, param.Name));
        }

        var object_type = delegate_type.DeclaringType;

        if (object_type is null)
            throw new InvalidOperationException("Unable to resolve declaring type for delegate");

        var best_constructor = object_type.GetConstructors().Where(c => CanResolveParametersForConstructor(c, method.GetParameters())).OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();

        if (best_constructor is null)
            throw new InvalidOperationException("Could not locate a suitable constructor");

        var constructor_parameters = new List<Expression>();

        foreach (var param in delegate_parameters)
        {
            constructor_parameters.Add(param);
        }

        foreach (var param in best_constructor.GetParameters().Skip(method.GetParameters().Length))
        {
            var resolved_parameter = Expression.Call(Expression.Constant(this), nameof(Get), new[] {param.ParameterType});
            constructor_parameters.Add(resolved_parameter);
        }

        var constructor = Expression.New(best_constructor, constructor_parameters);

        return Expression.Lambda(delegate_type, constructor, delegate_parameters).Compile();
    }

    private object Resolve(Type type, string? name)
    {
        if (type.IsDelegate())
            return BuildFactoryExpression(type);

        if (!CanResolveType(type))
            throw new ArgumentException("Can not resolve type {0}", type.FullName);

        if ((type == typeof(IContainer)) || (type == typeof(Container)))
            return this;

        TypeDescriptor? descriptor = null;

        if (type.IsGenericType)
        {
            var generic_type_definition = type.GetGenericTypeDefinition();
            descriptor = _types.First(t => (t.ConcreteType == generic_type_definition) || (t.InterfaceType == generic_type_definition));
        }
        else
        {
            if (name is null)
            {
                descriptor = _types.First(t => (t.ConcreteType == type) || (t.InterfaceType == type));
            }
            else
            {
                descriptor = _types.First(t =>
                    ((t.ConcreteType == type) || (t.InterfaceType == type)) && (t.Name == name));
            }
        }

        if (descriptor is null || descriptor.ConcreteType is null)
            throw new TypeNotRegisteredException(type);

        if (descriptor.Instance is not null)
            return descriptor.Instance;

        if (descriptor.ConcreteType.GetConstructors().Length == 0)
        {
            var new_instance = Activator.CreateInstance(descriptor.ConcreteType) ?? throw new InvalidOperationException("Could not create instance of type");

            if (descriptor.Lifetime == Lifetime.Singleton)
                _ = descriptor.WithInstance(new_instance);

            return new_instance;
        }

        var best_constructor = descriptor.ConcreteType.GetConstructors().Where(CanResolveParametersForConstructor).OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();

        if (best_constructor is null)
            throw new InvalidOperationException($"Could not locate a suitable constructor for type {descriptor.ConcreteType.FullName}");

        var parameters = best_constructor.GetParameters();

        if (parameters.Length == 0)
        {
            object? new_instance = null;
            
            if (type.IsGenericType)
            {
                Type constructedType = descriptor.ConcreteType.MakeGenericType(type.GenericTypeArguments);
                new_instance = Activator.CreateInstance(constructedType);
            }
            else
            {
                new_instance = Activator.CreateInstance(descriptor.ConcreteType);
            }

            if (new_instance is null)
                throw new InvalidOperationException("Could not create instance of type");

            if (descriptor.Lifetime == Lifetime.Singleton)
                _ = descriptor.WithInstance(new_instance);

            return new_instance;
        }

        var args = new object[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            args[i] = Resolve(parameters[i].ParameterType, null);
        }

        object? instance = null;

        if (type.IsGenericType)
        {
            Type constructedType = descriptor.ConcreteType.MakeGenericType(type.GenericTypeArguments);
            instance = Activator.CreateInstance(constructedType, args);
        }
        else
        {
            instance = Activator.CreateInstance(descriptor.ConcreteType, args);
        }
        
        if (instance is null) throw new InvalidOperationException("Could not create instance of type");

        if (descriptor.Lifetime == Lifetime.Singleton)

            // FIXME - We probably don't need to use the public interface of TypeDescriptor to set the instance.
            _ = descriptor.WithInstance(instance);

        return instance;
    }
}
