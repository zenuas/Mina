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
        EmitCall(il, method);
        EmitCast(il, typeof(R), method.ReturnType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, R>>();
    }

    public static Func<T, A, R> GetFunction<T, A, R>(MethodInfo method)
    {
        var parameters = method.GetParameters();

        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T), typeof(A)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        EmitLdarg(il, parameters[0].ParameterType, typeof(A), 1);
        EmitCall(il, method);
        EmitCast(il, typeof(R), method.ReturnType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, A, R>>();
    }

    public static Func<T, A1, A2, R> GetFunction<T, A1, A2, R>(MethodInfo method)
    {
        var parameters = method.GetParameters();

        var ilmethod = new DynamicMethod("", typeof(R), [typeof(T), typeof(A1), typeof(A2)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        EmitLdarg(il, parameters[0].ParameterType, typeof(A1), 1);
        EmitLdarg(il, parameters[1].ParameterType, typeof(A2), 2);
        EmitCall(il, method);
        EmitCast(il, typeof(R), method.ReturnType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, A1, A2, R>>();
    }

    public static Action<T> GetAction<T>(MethodInfo method)
    {
        var ilmethod = new DynamicMethod("", null, [typeof(T)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        EmitCall(il, method);
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
        EmitLdarg(il, parameters[0].ParameterType, typeof(A), 1);
        EmitCall(il, method);
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
        EmitLdarg(il, parameters[0].ParameterType, typeof(A1), 1);
        EmitLdarg(il, parameters[1].ParameterType, typeof(A2), 2);
        EmitCall(il, method);
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
        EmitCast(il, typeof(R), field.FieldType);
        il.Emit(OpCodes.Ret);
        return ilmethod.CreateDelegate<Func<T, R>>();
    }

    public static Action<T, A> SetField<T, A>(string name) where T : class
    {
        var field = typeof(T).GetField(name)!;

        var ilmethod = new DynamicMethod("", null, [typeof(T), typeof(A)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        EmitLdarg(il, field.FieldType, typeof(A), 1);
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

    public static void EmitLdarg(ILGenerator il, Type left_type, Type arg_type, int argn)
    {
        if (left_type == typeof(string) && arg_type.IsValueType)
        {
            il.Emit(OpCodes.Ldarga_S, argn);
            EmitCast(il, left_type, arg_type);
        }
        else
        {
            switch (argn)
            {
                case 0: il.Emit(OpCodes.Ldarg_0); break;
                case 1: il.Emit(OpCodes.Ldarg_1); break;
                case 2: il.Emit(OpCodes.Ldarg_2); break;
                case 3: il.Emit(OpCodes.Ldarg_3); break;
                default: il.Emit(OpCodes.Ldarg_S, argn); break;
            }
            EmitCast(il, left_type, arg_type);
        }
    }

    public static void EmitCall(ILGenerator il, MethodInfo method) => il.EmitCall(method.IsFinal || !method.IsVirtual ? OpCodes.Call : OpCodes.Callvirt, method, null);

    public static void EmitCastViaObject(ILGenerator il, Type left_type, Type right_type_wrap_object)
    {
        if (Nullable.GetUnderlyingType(left_type) is { } value_type)
        {
            var nullable = il.DeclareLocal(left_type);
            var right_value = il.DeclareLocal(typeof(object));
            var else_label = il.DefineLabel();
            var endif_label = il.DefineLabel();

            // var right_value = stack[top];
            il.Emit(OpCodes.Stloc, right_value);

            // if (right_value is value_type)
            il.Emit(OpCodes.Ldloc, right_value);
            il.Emit(OpCodes.Isinst, right_type_wrap_object);
            il.Emit(OpCodes.Brfalse_S, else_label);

            // then: nullable = Nullable<value_type>((value_type)right_value);
            il.Emit(OpCodes.Ldloca_S, nullable);
            il.Emit(OpCodes.Ldloc, right_value);
            il.Emit(OpCodes.Unbox_Any, right_type_wrap_object);
            EmitCast(il, value_type, right_type_wrap_object);
            il.Emit(OpCodes.Call, left_type.GetConstructor([value_type])!);
            il.Emit(OpCodes.Br_S, endif_label);

            // else: nullable = Nullable<value_type>();
            il.MarkLabel(else_label);
            il.Emit(OpCodes.Ldloca_S, nullable);
            il.Emit(OpCodes.Initobj, left_type);

            // endif: stack[top] = nullable;
            il.MarkLabel(endif_label);
            il.Emit(OpCodes.Ldloc, nullable);
        }
        else if (right_type_wrap_object.IsValueType)
        {
            EmitUnboxWithoutDBNull(il, right_type_wrap_object);
            EmitCast(il, left_type, right_type_wrap_object);
        }
        else
        {
            EmitCast(il, left_type, right_type_wrap_object);
        }
    }

    public static void EmitCast(ILGenerator il, Type left_type, Type right_type)
    {
        if (left_type == right_type) return;
        if (left_type == typeof(string))
        {
            var tostr = right_type.GetMethod("ToString", [])!;
            EmitCall(il, tostr);
        }
        else if (Nullable.GetUnderlyingType(left_type) is { } value_type)
        {
            var nullable = il.DeclareLocal(left_type);
            var right_value = il.DeclareLocal(typeof(object));
            var else_label = il.DefineLabel();
            var endif_label = il.DefineLabel();

            // var right_value = stack[top];
            il.Emit(OpCodes.Stloc, right_value);

            // if (right_value is value_type)
            il.Emit(OpCodes.Ldloc, right_value);
            il.Emit(OpCodes.Isinst, right_type);
            il.Emit(OpCodes.Brfalse_S, else_label);

            // then: nullable = Nullable<value_type>((value_type)right_value);
            il.Emit(OpCodes.Ldloca_S, nullable);
            il.Emit(OpCodes.Ldloc, right_value);
            il.Emit(OpCodes.Unbox_Any, right_type);
            EmitCast(il, value_type, right_type);
            il.Emit(OpCodes.Call, left_type.GetConstructor([value_type])!);
            il.Emit(OpCodes.Br_S, endif_label);

            // else: nullable = Nullable<value_type>();
            il.MarkLabel(else_label);
            il.Emit(OpCodes.Ldloca_S, nullable);
            il.Emit(OpCodes.Initobj, left_type);

            // endif: stack[top] = nullable;
            il.MarkLabel(endif_label);
            il.Emit(OpCodes.Ldloc, nullable);
        }
        else if (left_type.IsValueType && Nullable.GetUnderlyingType(right_type) is { } from_type)
        {
            // stack[top] = (from_type)stack[top].GetValueOrDefault();
            EmitCall(il, right_type.GetMethod("GetValueOrDefault", [])!);
            EmitCast(il, left_type, from_type);
        }
        else if (left_type.IsValueType && right_type == typeof(string))
        {
            EmitCall(il, left_type.GetMethod("Parse", [typeof(string)])!);
        }
        else if (left_type.IsValueType && right_type.IsValueType)
        {
            switch (left_type)
            {
                case Type a when a == typeof(int): il.Emit(OpCodes.Conv_I4); break;
                case Type a when a == typeof(long): il.Emit(OpCodes.Conv_I8); break;
                case Type a when a == typeof(short): il.Emit(OpCodes.Conv_I2); break;
                case Type a when a == typeof(sbyte): il.Emit(OpCodes.Conv_I1); break;

                case Type a when a == typeof(uint): il.Emit(OpCodes.Conv_U4); break;
                case Type a when a == typeof(ulong): il.Emit(OpCodes.Conv_U8); break;
                case Type a when a == typeof(ushort): il.Emit(OpCodes.Conv_U2); break;
                case Type a when a == typeof(byte): il.Emit(OpCodes.Conv_U1); break;
            }
        }
        else if (left_type.IsValueType && !right_type.IsValueType)
        {
            il.Emit(OpCodes.Unbox_Any, left_type);
        }
        else if (!left_type.IsValueType && right_type.IsValueType)
        {
            il.Emit(OpCodes.Box, right_type);
        }
        else
        {
            il.Emit(OpCodes.Castclass, left_type);
        }
    }

    public static Type? EmitLoad<T>(ILGenerator il, string name)
    {
        if (typeof(T).GetProperty(name)?.GetMethod is { } get_prop)
        {
            EmitCall(il, get_prop);
            return get_prop.ReturnType;
        }
        else if (typeof(T).GetMethod(name) is { } get_method)
        {
            EmitCall(il, get_method);
            return get_method.ReturnType;
        }
        else if (typeof(T).GetField(name) is { } load_field)
        {
            il.Emit(OpCodes.Ldfld, load_field);
            return load_field.FieldType;
        }
        return null;
    }

    public static bool EmitStore<T>(ILGenerator il, string name, Type parameter_type)
    {
        if (typeof(T).GetProperty(name)?.SetMethod is { } set_prop && set_prop.GetParameters() is { } prop_param && prop_param.Length == 1)
        {
            EmitCast(il, prop_param[0].ParameterType, parameter_type);
            il.Emit(OpCodes.Call, set_prop);
            return true;
        }
        else if (typeof(T).GetMethod(name) is { } set_method && set_method.GetParameters() is { } method_param && method_param.Length == 1)
        {
            EmitCast(il, method_param[0].ParameterType, parameter_type);
            il.Emit(OpCodes.Call, set_method);
            if (set_method.ReturnType != typeof(void)) il.Emit(OpCodes.Pop);
            return true;
        }
        else if (typeof(T).GetField(name) is { } store_field)
        {
            EmitCast(il, store_field.FieldType, parameter_type);
            il.Emit(OpCodes.Stfld, store_field);
            return true;
        }
        return false;
    }

    public static Type? WhenEmitStorable<T>(string name)
    {
        if (typeof(T).GetProperty(name)?.SetMethod is { } set_prop && set_prop.GetParameters() is { } prop_param && prop_param.Length == 1)
        {
            return prop_param[0].ParameterType;
        }
        else if (typeof(T).GetMethod(name) is { } set_method && set_method.GetParameters() is { } method_param && method_param.Length == 1)
        {
            return method_param[0].ParameterType;
        }
        else if (typeof(T).GetField(name) is { } store_field)
        {
            return store_field.FieldType;
        }
        return null;
    }

    public static void EmitUnboxWithoutDBNull(ILGenerator il, Type type)
    {
        // dup stack[top];
        il.Emit(OpCodes.Dup);

        // if (stack[top] is not DBNull)
        var endif_label = il.DefineLabel();
        il.Emit(OpCodes.Isinst, typeof(DBNull));
        il.Emit(OpCodes.Brtrue_S, endif_label);

        // then: stack[top] = (load_type)stack[top];
        il.Emit(OpCodes.Unbox_Any, type);
        il.MarkLabel(endif_label);
    }
}
