﻿using System;
using System.Collections.Generic;
using System.Linq;

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

    public static Dictionary<string, Action<T, A>> CreateSetMapper<T, A>() => typeof(T)
        .GetProperties()
        .Select(x => (x.Name, Method: x.GetSetMethod()!))
        .Where(x => x.Method.GetParameters() is { } ps && ps.Length == 1 && ps[0].ParameterType == typeof(A))
        .ToDictionary(x => x.Name, x => Expressions.GetAction<T, A>(x.Method));

    public static Dictionary<string, Action<T, dynamic>> CreateSetMapper<T>() => typeof(T)
        .GetProperties()
        .Select(x => (x.Name, Method: x.GetSetMethod()!))
        .Where(x => x.Method.GetParameters() is { } ps && ps.Length == 1)
        .ToDictionary(x => x.Name, x => Expressions.GetAction<T, dynamic>(x.Method));
}
