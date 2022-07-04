// // Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject;

public interface IContainer
{
    ITypeDescriptor Add<T>();
    ITypeDescriptor Add(object instance);
    ITypeDescriptor Add(Type type);

    T Get<T>();
    T Get<T>(string name);
    
}
