using Mina.Extension;
using Mina.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;

namespace Mina.Mapper;

public static class ObjectMapper
{
    public static Func<T, R> CreateMapper<T, R>(Dictionary<string, string> map, Action<ILGenerator, string, Type> loadf)
    {
        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        LocalBuilder? local = null;
        Dictionary<string, string>? newmap = null;

        if (typeof(R).IsValueType)
        {
            local = il.Ldloca<R>();
            il.Initobj<R>();
            il.Ldloca(local);
        }
        else if (typeof(R).GetConstructor([]) is { } ctor0)
        {
            il.Newobj(ctor0);
        }
        else if (typeof(R).GetConstructors() is { } ctors && ctors.Where(x => x.GetParameters().All(y => map.ContainsValue(y.Name!))).FirstOrDefault() is { } ctor)
        {
            var ctor_params = ctor.GetParameters();
            var ctor_map = map.Where(x => ctor_params.Contains(y => y.Name == x.Value)).ToDictionary(x => x.Value, x => x.Key);
            ctor_params.Each(x => loadf(il, ctor_map[x.Name!], x.ParameterType));
            il.Newobj(ctor);
            newmap = map.Where(x => !ctor_params.Contains(y => y.Name == x.Value)).ToDictionary();
        }
        else
        {
            throw new InvalidOperationException();
        }

        foreach (var (from_name, to_name) in newmap ?? map)
        {
            il.Emit(OpCodes.Dup);
            var store_type = ILGenerators.WhenStoreAnyPropertyType<R>(to_name)!;

            // stack[top] = (store_type)loadf(from_name);
            loadf(il, from_name, store_type);

            if (!il.StoreAnyProperty<R>(to_name, store_type)) throw new("destination not found");
        }
        if (local is { })
        {
            il.Emit(OpCodes.Pop);
            il.Ldloc(local);
        }
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, R>>();
    }

    public static Func<T, R> CreateMapper<T, R>(IEnumerable<string> map) => CreateMapper<T, R>(map.ToDictionary(x => x));

    public static Func<T, R> CreateMapper<T, R>(Dictionary<string, string> map)
    {
        return CreateMapper<T, R>(map, (il, name, type) =>
        {
            il.Ldarg(0);
            var load_type = il.LoadAnyProperty<T>(name) ?? throw new("source not found");
            if (ILGenerators.IsRequireAddressLoad(type, load_type))
            {
                var local = il.Stloc(load_type);
                il.Ldloca(local);
            }
            il.AnyCast(type, load_type);
        });
    }

    public static Func<IEnumerable<T>, IEnumerable<R>> CreateEnumerableMapper<T, R>(IEnumerable<string> map) => CreateEnumerableMapper<T, R>(map.ToDictionary(x => x));

    public static Func<IEnumerable<T>, IEnumerable<R>> CreateEnumerableMapper<T, R>(Dictionary<string, string> map)
    {
        var f = CreateMapper<T, R>(map);
        return (xs) => xs.Select(f);
    }
}
