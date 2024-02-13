using System;
using System.Reflection;

namespace Mina.Extension;

public static class Enums
{
    public static T? GetAttributeOrDefault<T>(this Enum e) where T : Attribute
    {
        var field = e.GetType().GetField(e.ToString());
        return field?.GetCustomAttribute<T>() is T attr ? attr : null;
    }
}
