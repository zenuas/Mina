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

    public static Func<T, R> CreateMapper<T, R>(IEnumerable<string> map) => CreateMapper<T, R>(map.ToDictionary(x => x));

    public static Func<T, R> CreateMapper<T, R>(Dictionary<string, string> map)
    {
        var ctor = typeof(R).GetConstructor([])!;

        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Newobj, ctor);
        foreach (var (from_name, to_name) in map)
        {
            il.Emit(OpCodes.Dup);
            il.Emit(OpCodes.Ldarg_0);

            var load_type = Expressions.EmitLoad<T>(il, from_name);
            if (load_type is null) throw new("source not found");
            if (!Expressions.EmitStore<R>(il, to_name, load_type)) throw new("destination not found");
        }
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, R>>();
    }

    public static Func<DataRow, R> CreateMapper<R>(DataTable table) => CreateMapper<R>(table, table.Columns
        .GetIterator()
        .OfType<DataColumn>()
        .Where(x => Expressions.WhenEmitStorable<R>(x.ColumnName))
        .ToDictionary(x => x.ColumnName, x => x.ColumnName));

    public static Func<DataRow, R> CreateMapper<R>(DataTable table, IEnumerable<string> map) => CreateMapper<R>(table, map.ToDictionary(x => x));

    public static Func<DataRow, R> CreateMapper<R>(DataTable table, Dictionary<string, string> map)
    {
        var ctor = typeof(R).GetConstructor([])!;
        var get_item = typeof(DataRow).GetProperty("Item", [typeof(string)])!.GetMethod!;

        var ilmethod = new DynamicMethod("", typeof(R), [typeof(DataRow)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Newobj, ctor);
        foreach (var (from_name, to_name) in map)
        {
            il.Emit(OpCodes.Dup);
            var load_type = table.Columns[from_name]!.DataType;

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldstr, from_name);
            il.Emit(OpCodes.Callvirt, get_item);
            if (load_type.IsValueType) il.Emit(OpCodes.Unbox_Any, load_type);

            if (!Expressions.EmitStore<R>(il, to_name, load_type)) throw new("destination not found");
        }
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<DataRow, R>>();
    }
}
