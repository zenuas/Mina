using Mina.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Mina.Extension;

public static class Enums
{
    public static T? GetAttributeOrDefault<T>(this Enum e) where T : Attribute => e.GetType().GetField(e.ToString())?.GetCustomAttribute<T>() is T attr ? attr : null;

    public static T? Parse<T>(string name) where T : struct, Enum => Enum.TryParse(typeof(T), name, out var e) ? (T)e : null;

    public static T? ParseWithAlias<T>(string name) where T : struct, Enum => Parse<T>(name) is { } e ? e : (typeof(T).GetFields()
        .Where(x => x.GetCustomAttribute<AliasAttribute>() is { } alias && alias.Name == name)
        .FirstOrDefault()
        ?.GetValue(null)
        ?.Cast<T>());
}
