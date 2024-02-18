using Mina.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Mina.Extension;

public static class Enums
{
    public static T? GetAttributeOrDefault<T>(this Enum e) where T : Attribute
    {
        var field = e.GetType().GetField(e.ToString());
        return field?.GetCustomAttribute<T>() is T attr ? attr : null;
    }

    public static T? Parse<T>(string name) where T : struct, Enum
    {
        return Enum.TryParse(typeof(T), name, out var e) ? (T)e : null;
    }

    public static T? ParseWithAlias<T>(string name) where T : struct, Enum
    {
        if (Parse<T>(name) is { } e) return e;
        return typeof(T).GetFields()
            .Where(x => x.GetCustomAttribute<AliasAttribute>() is { } alias && alias.Name == name)
            .FirstOrDefault()
            ?.GetValue(null)
            ?.Cast<T>();
    }
}
