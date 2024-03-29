﻿using System;
using System.Reflection.Emit;

namespace Mina.Reflection;

public static partial class ILGenerators
{
    public static bool IsRequireAddressLoad(Type left_type, Type right_type) => (
            (left_type == typeof(string) && right_type.IsValueType) ||
            (Nullable.GetUnderlyingType(left_type) is null && Nullable.GetUnderlyingType(right_type) is { }) ||
            (Nullable.GetUnderlyingType(left_type) is { } left_t && Nullable.GetUnderlyingType(right_type) is { } right_t && left_t != right_t)
        );

    public static void LdargCast(this ILGenerator il, Type left_type, Type arg_type, int argn)
    {
        if (IsRequireAddressLoad(left_type, arg_type))
        {
            il.Ldarga(argn);
        }
        else
        {
            il.Ldarg(argn);
        }
        il.AnyCast(left_type, arg_type);
    }

    public static void ChangeType(this ILGenerator il, Type type)
    {
        // stack[top] = (type)Convert.ChangeType(stack[top], type);
        il.Ldtoken(type);
        il.Call(typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
        il.Call(typeof(Convert).GetMethod(nameof(Convert.ChangeType), [typeof(object), typeof(Type)])!);
        il.Unbox_Any(type);
    }

    public static LocalBuilder StoreNullable(this ILGenerator il, Type left_nullable, Type left_nullable_t, LocalBuilder right_value, Type? right_type = null)
    {
        // nullable = Nullable<left_nullable_t>((left_nullable_t)(right_type)right_value);
        var nullable = il.Ldloca(left_nullable);
        il.Ldloc(right_value);
        if (right_type is { }) il.ChangeType(right_type);
        il.AnyCast(left_nullable_t, right_type ?? right_value.LocalType);
        il.Call(left_nullable.GetConstructor([left_nullable_t])!);

        return nullable;
    }

    public static void NullableCastViaObject(this ILGenerator il, Type left_nullable, Type left_nullable_t, Type right_type)
    {
        // var right_value = stack[top];
        var right_value = il.Stloc<object>();

        // if (right_value is null) goto goto_label;
        il.Ldloc(right_value);
        var goto_label = il.IfIsNullThenGoto_S();

        // if (right_value is DBNull) goto goto_label;
        il.Ldloc(right_value);
        _ = il.IfIsInstanceThenGoto_S<DBNull>(goto_label);

        // nullable = Nullable<left_nullable_t>(right_value);
        var nullable = il.StoreNullable(left_nullable, left_nullable_t, right_value, right_type);
        var endif_label = il.Br_S();

        // goto_label: nullable = Nullable<nullable_t>();
        il.MarkLabel(goto_label);
        il.Ldloca(nullable);
        il.Initobj(left_nullable);

        // endif: stack[top] = nullable;
        il.MarkLabel(endif_label);
        il.Ldloc(nullable);
    }

    public static void NullableCast(this ILGenerator il, Type left_nullable, Type left_nullable_t, Type right_nullable, Type right_nullable_t)
    {
        // dup stack[top];
        il.Emit(OpCodes.Dup);

        // if (stack[top].HasValue)
        il.Call(right_nullable.GetProperty("HasValue")!.GetGetMethod()!);
        var else_label = il.IfTrueElseGoto();

        // then: nullable = Nullable<left_nullable_t>(right_value = stack[top].GetValueOrDefault());
        il.Call(right_nullable.GetMethod("GetValueOrDefault", [])!);
        var right_value = il.Stloc(right_nullable_t);
        var nullable = il.StoreNullable(left_nullable, left_nullable_t, right_value);
        var endif_label = il.Br_S();

        // else: nullable = Nullable<nullable_t>();
        il.MarkLabel(else_label);
        il.Emit(OpCodes.Pop);
        il.Ldloca(nullable);
        il.Initobj(left_nullable);

        // endif: stack[top] = nullable;
        il.MarkLabel(endif_label);
        il.Ldloc(nullable);
    }

    public static void CastViaObject(this ILGenerator il, Type left_type, Type right_type_wrap_object)
    {
        if (Nullable.GetUnderlyingType(left_type) is { } value_type)
        {
            il.NullableCastViaObject(left_type, value_type, right_type_wrap_object);
        }
        else if (right_type_wrap_object.IsValueType)
        {
            // if (stack[top] is DBNull)
            il.Emit(OpCodes.Dup);
            var else_label = il.IfIsInstanceElseGoto_S<DBNull>();

            // then: stack[top] = 0 or null;
            il.Emit(OpCodes.Pop);
            if (left_type.IsValueType)
            {
                il.Ldc_I4(0);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
            var endif_label = il.Br_S();

            // else: stack[top] = (load_type)stack[top];
            il.MarkLabel(else_label);
            il.Unbox_Any(right_type_wrap_object);

            // endif: statck[top] = (left_type)statck[top];
            il.MarkLabel(endif_label);
            il.AnyCast(left_type, right_type_wrap_object);
        }
        else
        {
            il.AnyCast(left_type, typeof(object));
        }
    }

    public static void AnyCast(this ILGenerator il, Type left_type, Type right_type)
    {
        if (left_type == right_type) return;
        if (left_type == typeof(string))
        {
            if (right_type.IsValueType)
            {
                if (Nullable.GetUnderlyingType(right_type) is { } right_nullable_t)
                {
                    // if (stack[top].HasValue)
                    il.Emit(OpCodes.Dup);
                    il.Call(right_type.GetProperty("HasValue")!.GetGetMethod()!);
                    var else_label = il.IfTrueElseGoto();

                    // then: stack[top] = stack[top].Value.ToString();
                    il.Call(right_type.GetProperty("Value")!.GetGetMethod()!);
                    var right_value = il.Stloc(right_nullable_t);
                    il.Ldloca(right_value);
                    il.Call(right_nullable_t.GetMethod("ToString", [])!);
                    var endif_label = il.Br_S();

                    // else: stack[top] = null;
                    il.MarkLabel(else_label);
                    il.Emit(OpCodes.Pop);
                    il.Emit(OpCodes.Ldnull);

                    il.MarkLabel(endif_label);
                }
                else
                {
                    // stack[top] = stack[top].ToString();
                    il.Call(right_type.GetMethod("ToString", [])!);
                }
            }
            else
            {
                // if (stack[top] is null) goto endif_label;
                il.Emit(OpCodes.Dup);
                var endif_label = il.IfIsNullThenGoto_S();

                // if (stack[top] is DBNull)
                il.Emit(OpCodes.Dup);
                var else_label = il.IfIsInstanceElseGoto_S<DBNull>();

                // then: stack[top] = null;
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ldnull);
                il.Br_S(endif_label);

                // else: stack[top] = stack[top].ToString();
                il.MarkLabel(else_label);
                il.Call(right_type.GetMethod("ToString", [])!);

                il.MarkLabel(endif_label);
            }
        }
        else if (Nullable.GetUnderlyingType(left_type) is { } left_nullable_t)
        {
            if (Nullable.GetUnderlyingType(right_type) is { } right_nullable_t)
            {
                il.NullableCast(left_type, left_nullable_t, right_type, right_nullable_t);
            }
            else if (right_type.IsValueType)
            {
                // right_value = (value_type)stack[top];
                il.AnyCast(left_nullable_t, right_type);
                var right_value = il.Stloc(right_type);

                // nullable = Nullable<nullable_t>(right_value);
                var nullable = il.StoreNullable(left_type, left_nullable_t, right_value);

                // stack[top] = nullable;
                il.Ldloc(nullable);
            }
            else if (right_type == typeof(string))
            {
                // stack[top] = (left_type)(left_nullable_t)stack[top];
                il.ChangeType(left_nullable_t);
                il.AnyCast(left_type, left_nullable_t);
            }
            else
            {
                // if (stack[top] is string)
                il.Emit(OpCodes.Dup);
                var else_label = il.IfIsInstanceElseGoto_S<string>();

                // then: stack[top] = (left_type)(string)stack[top];
                il.AnyCast(left_type, typeof(string));
                var endif_label = il.Br_S();

                // else: stack[top] = (left_type)stack[top];
                il.MarkLabel(else_label);
                il.NullableCastViaObject(left_type, left_nullable_t, left_nullable_t);

                il.MarkLabel(endif_label);
            }
        }
        else if (left_type.IsValueType)
        {
            if (Nullable.GetUnderlyingType(right_type) is { } right_nullable_t)
            {
                // stack[top] = (from_type)stack[top].GetValueOrDefault();
                il.Call(right_type.GetMethod("GetValueOrDefault", [])!);
                il.AnyCast(left_type, right_nullable_t);
            }
            else if (right_type.IsValueType)
            {
                // stack[top] = (left_type)stack[top];
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
            else if (right_type == typeof(string))
            {
                // stack[top] = (left_type)stack[top]);
                il.ChangeType(left_type);
            }
            else
            {
                // if (stack[top] is null)
                il.Emit(OpCodes.Dup);
                var else1_label = il.IfIsNullElseGoto_S();

                // then: stack[top] = 0;
                il.Emit(OpCodes.Pop);
                il.Ldc_I4(0);
                var endif_label = il.Br_S();

                // else: if (stack[top] is DBNull)
                il.MarkLabel(else1_label);
                il.Emit(OpCodes.Dup);
                var else2_label = il.IfIsInstanceElseGoto_S<DBNull>();

                // then: stack[top] = 0;
                il.Emit(OpCodes.Pop);
                il.Ldc_I4(0);
                il.Br_S(endif_label);

                // else: if (stack[top] is string)
                il.MarkLabel(else2_label);
                il.Emit(OpCodes.Dup);
                var else3_label = il.IfIsInstanceElseGoto_S<string>();

                // then: stack[top] = (left_type)(string)stack[top];
                il.AnyCast(left_type, typeof(string));
                il.Br_S(endif_label);

                // else: stack[top] = (left_type)stack[top];
                il.MarkLabel(else3_label);
                il.ChangeType(left_type);

                il.MarkLabel(endif_label);
            }
        }
        else if (!left_type.IsValueType && right_type.IsValueType)
        {
            // stack[top] = (object)stack[top];
            il.Box(right_type);
        }
        else
        {
            // stack[top] = (left_type)stack[top];
            il.Castclass(left_type);
        }
    }

    public static Type? LoadAnyProperty<T>(this ILGenerator il, string name)
    {
        if (typeof(T).GetProperty(name)?.GetMethod is { } get_prop)
        {
            il.Call(get_prop);
            return get_prop.ReturnType;
        }
        else if (typeof(T).GetMethod(name) is { } get_method)
        {
            il.Call(get_method);
            return get_method.ReturnType;
        }
        else if (typeof(T).GetField(name) is { } load_field)
        {
            il.Ldfld(load_field);
            return load_field.FieldType;
        }
        return null;
    }

    public static bool StoreAnyProperty<T>(this ILGenerator il, string name, Type parameter_type)
    {
        if (typeof(T).GetProperty(name)?.SetMethod is { } set_prop && set_prop.GetParameters() is { } prop_param && prop_param.Length == 1)
        {
            il.AnyCast(prop_param[0].ParameterType, parameter_type);
            il.Call(set_prop);
            return true;
        }
        else if (typeof(T).GetMethod(name) is { } set_method && set_method.GetParameters() is { } method_param && method_param.Length == 1)
        {
            il.AnyCast(method_param[0].ParameterType, parameter_type);
            il.Call(set_method);
            if (set_method.ReturnType != typeof(void)) il.Emit(OpCodes.Pop);
            return true;
        }
        else if (typeof(T).GetField(name) is { } store_field)
        {
            il.AnyCast(store_field.FieldType, parameter_type);
            il.Stfld(store_field);
            return true;
        }
        return false;
    }

    public static Type? WhenStoreAnyPropertyType<T>(string name)
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
}
