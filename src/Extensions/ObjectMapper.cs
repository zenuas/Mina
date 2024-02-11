using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

    public static (LocalBuilder? Local, Dictionary<string, string>? NewMap) EmitCreateMapperInstance<T>(ILGenerator il, Dictionary<string, string> map, Action<string, ParameterInfo> loadf)
    {
        LocalBuilder? local = null;
        Dictionary<string, string>? newmap = null;

        if (typeof(T).IsValueType)
        {
            local = il.DeclareLocal(typeof(T));
            il.Emit(OpCodes.Ldloca_S, local);
            il.Emit(OpCodes.Initobj, typeof(T));
            il.Emit(OpCodes.Ldloca_S, local);
        }
        else if (typeof(T).GetConstructor([]) is { } ctor0)
        {
            il.Emit(OpCodes.Newobj, ctor0);
        }
        else if (typeof(T).GetConstructors() is { } ctors && ctors.Where(x => x.GetParameters().All(y => map.ContainsValue(y.Name!))).FirstOrDefault() is { } ctor)
        {
            var ctor_params = ctor.GetParameters();
            var ctor_map = map.Where(x => ctor_params.Contains(y => y.Name == x.Value)).ToDictionary(x => x.Value, x => x.Key);
            ctor_params.Each(x => loadf(ctor_map[x.Name!], x));
            il.Emit(OpCodes.Newobj, ctor);
            newmap = map.Where(x => !ctor_params.Contains(y => y.Name == x.Value)).ToDictionary();
        }
        else
        {
            throw new InvalidOperationException();
        }
        return (local, newmap);
    }

    public static Func<T, R> CreateMapper<T, R>(IEnumerable<string> map) => CreateMapper<T, R>(map.ToDictionary(x => x));

    public static Func<T, R> CreateMapper<T, R>(Dictionary<string, string> map)
    {
        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        var (local, newmap) = EmitCreateMapperInstance<R>(il, map, (name, _) =>
        {
            il.Emit(OpCodes.Ldarg_0);
            Reflections.EmitLoad<T>(il, name);
        });

        foreach (var (from_name, to_name) in newmap ?? map)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_0);

            var load_type = Reflections.EmitLoad<T>(il, from_name);
            if (load_type is null) throw new("source not found");
            if (!Reflections.EmitStore<R>(il, to_name, load_type)) throw new("destination not found");
        }
        if (typeof(R).IsValueType)
        {
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ldloc, local!);
        }
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, R>>();
    }

    public static Func<DataRow, T> CreateMapper<T>(DataTable table) => CreateMapper<T>(table, table.Columns
        .GetIterator()
        .OfType<DataColumn>()
        .Where(x => Reflections.WhenEmitStorable<T>(x.ColumnName) is { })
        .ToDictionary(x => x.ColumnName, x => x.ColumnName));

    public static Func<DataRow, T> CreateMapper<T>(DataTable table, IEnumerable<string> map) => CreateMapper<T>(table, map.ToDictionary(x => x));

    public static Func<DataRow, T> CreateMapper<T>(DataTable table, Dictionary<string, string> map)
    {
        var get_item = typeof(DataRow).GetProperty("Item", [typeof(string)])!.GetMethod!;

        var ilmethod = new DynamicMethod("", typeof(T), [typeof(DataRow)]);
        var il = ilmethod.GetILGenerator();
        var (local, newmap) = EmitCreateMapperInstance<T>(il, map, (name, info) =>
        {
            var load_type = table.Columns[name]!.DataType;

            // stack[top] = (store_type)arg0[from_name];
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldstr, name);
            il.Emit(OpCodes.Callvirt, get_item);
            Reflections.EmitCastViaObject(il, info.ParameterType, load_type);
        });

        foreach (var (from_name, to_name) in newmap ?? map)
        {
            il.Emit(OpCodes.Dup);
            var store_type = Reflections.WhenEmitStorable<T>(to_name)!;
            var load_type = table.Columns[from_name]!.DataType;

            // stack[top] = (store_type)arg0[from_name];
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldstr, from_name);
            il.Emit(OpCodes.Callvirt, get_item);
            Reflections.EmitCastViaObject(il, store_type, load_type);

            if (!Reflections.EmitStore<T>(il, to_name, store_type)) throw new("destination not found");
        }
        if (typeof(T).IsValueType)
        {
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ldloc, local!);
        }
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<DataRow, T>>();
    }

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader) => CreateMapper<T>(reader, Enumerable.Range(0, reader.FieldCount)
        .Select(x => reader.GetName(x))
        .Where(x => Reflections.WhenEmitStorable<T>(x) is { })
        .ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader, IEnumerable<string> map) => CreateMapper<T>(reader, map.ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader, Dictionary<string, string> map)
    {
        var ilmethod = new DynamicMethod("", typeof(T), [typeof(object[])]);
        var il = ilmethod.GetILGenerator();
        var (local, newmap) = EmitCreateMapperInstance<T>(il, map, (name, info) =>
        {
            var index = reader.GetOrdinal(name);
            var load_type = reader.GetFieldType(index);

            // stack[top] = (store_type)arg0[index];
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, index);
            il.Emit(OpCodes.Ldelem_Ref);
            Reflections.EmitCastViaObject(il, info.ParameterType, load_type);
        });

        foreach (var (from_name, to_name) in newmap ?? map)
        {
            il.Emit(OpCodes.Dup);
            var index = reader.GetOrdinal(from_name);
            var store_type = Reflections.WhenEmitStorable<T>(to_name)!;
            var load_type = reader.GetFieldType(index);

            // stack[top] = (store_type)arg0[index];
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, index);
            il.Emit(OpCodes.Ldelem_Ref);
            Reflections.EmitCastViaObject(il, store_type, load_type);

            if (!Reflections.EmitStore<T>(il, to_name, store_type)) throw new("destination not found");
        }
        if (typeof(T).IsValueType)
        {
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ldloc, local!);
        }
        il.Emit(OpCodes.Ret);

        static IEnumerable<T> ReadMapper(IDataReader reader, Func<object[], T> mapper)
        {
            var buffer = new object[reader.FieldCount];
            while (reader.Read())
            {
                _ = reader.GetValues(buffer);
                yield return mapper(buffer);
            }
        }
        return (arg) => ReadMapper(arg, ilmethod.CreateDelegate<Func<object[], T>>());
    }
}
