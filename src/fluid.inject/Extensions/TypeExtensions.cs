// Copyright (c) 2022, Nick Alcorso <nalcorso@gmail.com>

namespace Fluid.Inject.Extensions;

public static class TypeExtensions
{
    public static bool IsDelegate(this Type type)
    {
        return type.IsSubclassOf(typeof(Delegate)) || (type == typeof(Delegate));
    }
}
