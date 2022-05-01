// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject;

public interface IContainer
{
    T Get<T>() where T : class;

    bool Has(Type type);
    bool Has(Type interface_type, Type instance_type);

    IContainer AddSingleton<T>() where T : class;
    IContainer AddSingleton<I, T>() where I : class where T : class, I;
    IContainer AddSingleton<T>(T instance) where T : class;

    IContainer AddTransient<T>() where T : class;
    IContainer AddTransient<I, T>() where I : class where T : class, I;

    IContainer AddNamedSingleton<T>(string name) where T : class;
    IContainer AddNamedSingleton<I, T>(string name) where I : class where T : class, I;
    IContainer AddNamedSingleton<T>(string name, T instance) where T : class;

    IContainer AddNamedTransient<T>(string name) where T : class;
    IContainer AddNamedTransient<I, T>(string name) where I : class where T : class, I;
}
