﻿using Mina.Reflections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;

namespace Mina.Extensions;

public static class ObjectMapper
{
    public static Dictionary<string, Func<T, R>> CreateGetMapper<T, R>() => typeof(T)
        .GetProperties()
        .Select(x => (x.Name, Method: x.GetGetMethod()!))
        .Where(x => x.Method.GetParameters().Length == 0 && x.Method.ReturnType == typeof(R))
        .ToDictionary(x => x.Name, x => Expressions.GetFunction<T, R>(x.Method));

    public static Dictionary<string, Func<T, object>> CreateGetMapper<T>() => typeof(T)
        .GetProperties()
        .Select(x => (x.Name, Method: x.GetGetMethod()!))
        .Where(x => x.Method.GetParameters().Length == 0)
        .ToDictionary(x => x.Name, x => Expressions.GetFunction<T, object>(x.Method));

    public static Dictionary<string, Action<T, A>> CreateSetMapper<T, A>(bool parameter_type_check = true) where T : class => typeof(T)
        .GetProperties()
        .Select(x => (x.Name, Method: x.GetSetMethod()!))
        .Where(x => x.Method.GetParameters() is { } ps && ps.Length == 1 && (!parameter_type_check || ps[0].ParameterType == typeof(A)))
        .ToDictionary(x => x.Name, x => Expressions.GetAction<T, A>(x.Method));

    public static Dictionary<string, Action<T, dynamic>> CreateSetMapper<T>() where T : class => typeof(T)
        .GetProperties()
        .Select(x => (x.Name, Method: x.GetSetMethod()!))
        .Where(x => x.Method.GetParameters() is { } ps && ps.Length == 1)
        .ToDictionary(x => x.Name, x => Expressions.GetAction<T, dynamic>(x.Method));

    public static Dictionary<string, Func<T, R>> CreateFieldGetMapper<T, R>() => typeof(T)
        .GetFields()
        .Where(x => x.FieldType == typeof(R))
        .ToDictionary(x => x.Name, x => Expressions.GetField<T, R>(x.Name));

    public static Dictionary<string, Func<T, object>> CreateFieldGetMapper<T>() => typeof(T)
        .GetFields()
        .ToDictionary(x => x.Name, x => Expressions.GetField<T, dynamic>(x.Name));

    public static Dictionary<string, Action<T, A>> CreateFieldSetMapper<T, A>() where T : class => typeof(T)
        .GetFields()
        .Where(x => x.FieldType == typeof(A))
        .ToDictionary(x => x.Name, x => Expressions.SetField<T, A>(x.Name));

    public static Dictionary<string, Action<T, object>> CreateFieldSetMapper<T>() where T : class => typeof(T)
        .GetFields()
        .ToDictionary(x => x.Name, x => Expressions.SetField<T, dynamic>(x.Name));

    public static Func<T, R> CreateMapper<T, R>(Dictionary<string, string> map, Action<ILGenerator, string, Type> loadf)
    {
        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        LocalBuilder? local = null;
        Dictionary<string, string>? newmap = null;

        if (typeof(R).IsValueType)
        {
            local = il.DeclareLocal(typeof(R));
            il.Ldloca(local);
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
                var local = il.DeclareLocal(load_type);
                il.Stloc(local);
                il.Ldloca(local);
            }
            il.AnyCast(type, load_type);
        });
    }

    public static Func<DataRow, T> CreateMapper<T>(DataTable table) => CreateMapper<T>(table, table.Columns
        .GetIterator()
        .OfType<DataColumn>()
        .Where(x => ILGenerators.WhenStoreAnyPropertyType<T>(x.ColumnName) is { })
        .ToDictionary(x => x.ColumnName, x => x.ColumnName));

    public static Func<DataRow, T> CreateMapper<T>(DataTable table, IEnumerable<string> map) => CreateMapper<T>(table, map.ToDictionary(x => x));

    public static Func<DataRow, T> CreateMapper<T>(DataTable table, Dictionary<string, string> map)
    {
        var get_item = typeof(DataRow).GetProperty("Item", [typeof(string)])!.GetMethod!;

        return CreateMapper<DataRow, T>(map, (il, name, type) =>
        {
            var load_type = table.Columns[name]!.DataType;

            // stack[top] = (store_type)arg0[from_name];
            il.Ldarg(0);
            il.Ldstr(name);
            il.Call(get_item);
            il.CastViaObject(type, load_type);
        });
    }

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader) => CreateMapper<T>(reader, Enumerable.Range(0, reader.FieldCount)
        .Select(x => reader.GetName(x))
        .Where(x => ILGenerators.WhenStoreAnyPropertyType<T>(x) is { })
        .ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader, IEnumerable<string> map) => CreateMapper<T>(reader, map.ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader, Dictionary<string, string> map)
    {
        var f = CreateMapper<object[], T>(map, (il, name, type) =>
        {
            var index = reader.GetOrdinal(name);
            var load_type = reader.GetFieldType(index);

            // stack[top] = (store_type)arg0[index];
            il.Ldarg(0);
            il.Ldc_I4(index);
            il.Emit(OpCodes.Ldelem_Ref);
            il.CastViaObject(type, load_type);
        });
        static IEnumerable<T> ReadMapper(IDataReader reader, Func<object[], T> mapper)
        {
            var buffer = new object[reader.FieldCount];
            while (reader.Read())
            {
                _ = reader.GetValues(buffer);
                yield return mapper(buffer);
            }
        }
        return (arg) => ReadMapper(arg, f);
    }
}
