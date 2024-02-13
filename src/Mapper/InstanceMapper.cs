using Mina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mina.Mapper;

public static class InstanceMapper
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
}
