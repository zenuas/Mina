using System;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;

namespace Mina.Extensions;

public static class Expressions
{
    public static Func<T, T, T> Add<T>() where T : IAdditionOperators<T, T, T> => (a, b) => a + b;

    public static Func<T, T, T> Subtract<T>() where T : ISubtractionOperators<T, T, T> => (a, b) => a - b;

    public static Func<T, T, T> Multiply<T>() where T : IMultiplyOperators<T, T, T> => (a, b) => a * b;

    public static Func<T, T, T> Divide<T>() where T : IDivisionOperators<T, T, T> => (a, b) => a / b;

    public static Func<T, T, T> Modulo<T>() where T : IModulusOperators<T, T, T> => (a, b) => a % b;

    public static Func<T, T, T> LeftShift<T>() where T : IShiftOperators<T, T, T> => (a, b) => a << b;

    public static Func<T, T, T> RightShift<T>() where T : IShiftOperators<T, T, T> => (a, b) => a >> b;

    public static Func<T, R> GetProperty<T, R>(string name) => GetFunction<T, R>(typeof(T).GetProperty(name)!.GetMethod!);

    public static Action<T, A> SetProperty<T, A>(string name) => GetAction<T, A>(typeof(T).GetProperty(name)!.SetMethod!);

    public static Func<T, R> GetFunction<T, R>(string name) => GetFunction<T, R>(typeof(T).GetMethod(name)!);

    public static Func<T, A, R> GetFunction<T, A, R>(string name) => GetFunction<T, A, R>(typeof(T).GetMethod(name)!);

    public static Func<T, A1, A2, R> GetFunction<T, A1, A2, R>(string name) => GetFunction<T, A1, A2, R>(typeof(T).GetMethod(name)!);

    public static Action<T> GetAction<T>(string name) => GetAction<T>(typeof(T).GetMethod(name)!);

    public static Action<T, A> GetAction<T, A>(string name) => GetAction<T, A>(typeof(T).GetMethod(name)!);

    public static Action<T, A1, A2> GetAction<T, A1, A2>(string name) => GetAction<T, A1, A2>(typeof(T).GetMethod(name)!);

    public static Func<T> GetNew<T>() => GetNew<T>(typeof(T).GetConstructor([])!);

    public static Func<T, R> GetFunction<T, R>(MethodInfo method)
    {
        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        Reflections.EmitCall(il, method);
        Reflections.EmitCast(il, typeof(R), method.ReturnType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, R>>();
    }

    public static Func<T, A, R> GetFunction<T, A, R>(MethodInfo method)
    {
        var parameters = method.GetParameters();

        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T), typeof(A)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        Reflections.EmitLdarg(il, parameters[0].ParameterType, typeof(A), 1);
        Reflections.EmitCall(il, method);
        Reflections.EmitCast(il, typeof(R), method.ReturnType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, A, R>>();
    }

    public static Func<T, A1, A2, R> GetFunction<T, A1, A2, R>(MethodInfo method)
    {
        var parameters = method.GetParameters();

        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T), typeof(A1), typeof(A2)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        Reflections.EmitLdarg(il, parameters[0].ParameterType, typeof(A1), 1);
        Reflections.EmitLdarg(il, parameters[1].ParameterType, typeof(A2), 2);
        Reflections.EmitCall(il, method);
        Reflections.EmitCast(il, typeof(R), method.ReturnType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, A1, A2, R>>();
    }

    public static Action<T> GetAction<T>(MethodInfo method)
    {
        var ilmethod = new DynamicMethod("", null, [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        Reflections.EmitCall(il, method);
        if (method.ReturnType != typeof(void)) il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Action<T>>();
    }

    public static Action<T, A> GetAction<T, A>(MethodInfo method)
    {
        var parameters = method.GetParameters();

        var ilmethod = new DynamicMethod("", null, [typeof(T), typeof(A)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        Reflections.EmitLdarg(il, parameters[0].ParameterType, typeof(A), 1);
        Reflections.EmitCall(il, method);
        if (method.ReturnType != typeof(void)) il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Action<T, A>>();
    }

    public static Action<T, A1, A2> GetAction<T, A1, A2>(MethodInfo method)
    {
        var parameters = method.GetParameters();

        var ilmethod = new DynamicMethod("", null, [typeof(T), typeof(A1), typeof(A2)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        Reflections.EmitLdarg(il, parameters[0].ParameterType, typeof(A1), 1);
        Reflections.EmitLdarg(il, parameters[1].ParameterType, typeof(A2), 2);
        Reflections.EmitCall(il, method);
        if (method.ReturnType != typeof(void)) il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Action<T, A1, A2>>();
    }

    public static Func<T, R> GetField<T, R>(string name)
    {
        var field = typeof(T).GetField(name)!;

        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldfld, field);
        Reflections.EmitCast(il, typeof(R), field.FieldType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, R>>();
    }

    public static Action<T, A> SetField<T, A>(string name) where T : class
    {
        var field = typeof(T).GetField(name)!;

        var ilmethod = new DynamicMethod("", null, [typeof(T), typeof(A)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        Reflections.EmitLdarg(il, field.FieldType, typeof(A), 1);
        il.Emit(OpCodes.Stfld, field);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Action<T, A>>();
    }

    public static Func<T> GetNew<T>(ConstructorInfo ctor)
    {
        var ilmethod = new DynamicMethod("", typeof(T), []);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Newobj, ctor);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T>>();
    }
}
