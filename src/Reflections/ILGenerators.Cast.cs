using Mina.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Mina.Reflections;

public static partial class ILGenerators
{
    public static void EmitLdarg(ILGenerator il, Type left_type, Type arg_type, int argn)
    {
        if (left_type == typeof(string) && arg_type.IsValueType)
        {
            il.Ldarga(argn);
            EmitCast(il, left_type, arg_type);
        }
        else
        {
            il.Ldarg(argn);
            EmitCast(il, left_type, arg_type);
        }
    }

    public static void EmitChangeType(ILGenerator il, Type type)
    {
        // stack[top] = (type)Convert.ChangeType(stack[top], type);
        il.Emit(OpCodes.Ldtoken, type);
        il.Call(typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
        il.Call(typeof(Convert).GetMethod(nameof(Convert.ChangeType), [typeof(object), typeof(Type)])!);
        il.Emit(OpCodes.Unbox_Any, type);
    }

    public static LocalBuilder EmitStoreNullable(ILGenerator il, Type left_nullable, Type left_nullable_t, LocalBuilder right_value, Type? right_type = null)
    {
        var nullable = il.DeclareLocal(left_nullable);

        // nullable = Nullable<left_nullable_t>((left_nullable_t)(right_type)right_value);
        il.Ldloca(nullable);
        il.Ldloc(right_value);
        if (right_type is { }) EmitChangeType(il, right_type);
        EmitCast(il, left_nullable_t, right_type ?? right_value.LocalType);
        il.Emit(OpCodes.Call, left_nullable.GetConstructor([left_nullable_t])!);

        return nullable;
    }

    public static Label EmitIfIsInstanceThenGoto<T>(ILGenerator il, Label? then_label = null, LocalBuilder? local = null) => (then_label ?? il.DefineLabel()).Return(x => EmitIfIsInstanceGoto<T>(il, OpCodes.Brtrue_S, x, local));
    public static Label EmitIfIsInstanceElseGoto<T>(ILGenerator il, Label? else_label = null, LocalBuilder? local = null) => (else_label ?? il.DefineLabel()).Return(x => EmitIfIsInstanceGoto<T>(il, OpCodes.Brfalse_S, x, local));
    public static Label EmitIfIsNotInstanceThenGoto<T>(ILGenerator il, Label? then_label = null, LocalBuilder? local = null) => (then_label ?? il.DefineLabel()).Return(x => EmitIfIsInstanceGoto<T>(il, OpCodes.Brfalse_S, x, local));
    public static Label EmitIfIsNotInstanceElseGoto<T>(ILGenerator il, Label? else_label = null, LocalBuilder? local = null) => (else_label ?? il.DefineLabel()).Return(x => EmitIfIsInstanceGoto<T>(il, OpCodes.Brtrue_S, x, local));

    public static void EmitIfIsInstanceGoto<T>(ILGenerator il, OpCode br, Label goto_label, LocalBuilder? local = null)
    {
        // if (local is T) goto goto_label;
        if (local is { })
        {
            il.Ldloc(local);
        }
        else
        {
            il.Emit(OpCodes.Dup);
        }
        il.Isinst<T>();
        il.Emit(br, goto_label);
    }

    public static Label EmitIfIsNullThenGoto(ILGenerator il, Label? then_label = null, LocalBuilder? local = null) => (then_label ?? il.DefineLabel()).Return(x => EmitIfIsNullGoto(il, OpCodes.Brtrue_S, x, local));
    public static Label EmitIfIsNullElseGoto(ILGenerator il, Label? else_label = null, LocalBuilder? local = null) => (else_label ?? il.DefineLabel()).Return(x => EmitIfIsNullGoto(il, OpCodes.Brfalse_S, x, local));
    public static Label EmitIfIsNotNullThenGoto(ILGenerator il, Label? then_label = null, LocalBuilder? local = null) => (then_label ?? il.DefineLabel()).Return(x => EmitIfIsNullGoto(il, OpCodes.Brfalse_S, x, local));
    public static Label EmitIfIsNotNullElseGoto(ILGenerator il, Label? else_label = null, LocalBuilder? local = null) => (else_label ?? il.DefineLabel()).Return(x => EmitIfIsNullGoto(il, OpCodes.Brtrue_S, x, local));

    public static void EmitIfIsNullGoto(ILGenerator il, OpCode br, Label goto_label, LocalBuilder? local = null)
    {
        // if (local is null) goto goto_label;
        if (local is { })
        {
            il.Ldloc(local);
        }
        else
        {
            il.Emit(OpCodes.Dup);
        }
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Ceq);
        il.Emit(br, goto_label);
    }

    public static void EmitNullableCastViaObject(ILGenerator il, Type left_nullable, Type left_nullable_t, Type right_type)
    {
        var right_value = il.DeclareLocal(typeof(object));

        // var right_value = stack[top];
        il.Stloc(right_value);

        // if (right_value is null) goto goto_label;
        var goto_label = EmitIfIsNullThenGoto(il, local: right_value);

        // if (right_value is DBNull) goto goto_label;
        _ = EmitIfIsInstanceThenGoto<DBNull>(il, goto_label, right_value);

        // then: nullable = Nullable<left_nullable_t>(right_value);
        var nullable = EmitStoreNullable(il, left_nullable, left_nullable_t, right_value, right_type);
        var endif_label = il.Br_S();

        // goto_label: nullable = Nullable<nullable_t>();
        il.MarkLabel(goto_label);
        il.Ldloca(nullable);
        il.Emit(OpCodes.Initobj, left_nullable);

        // endif: stack[top] = nullable;
        il.MarkLabel(endif_label);
        il.Ldloc(nullable);
    }

    public static void EmitNullableCast(ILGenerator il, Type left_nullable, Type left_nullable_t, Type right_nullable, Type right_nullable_t)
    {
        var right_value = il.DeclareLocal(right_nullable_t);

        // dup stack[top];
        il.Emit(OpCodes.Dup);

        // if (stack[top].HasValue)
        il.Call(right_nullable.GetProperty("HasValue")!.GetGetMethod()!);
        var else_label = il.IfTrueElseGoto();

        // then: nullable = Nullable<left_nullable_t>(right_value = stack[top].GetValueOrDefault());
        il.Call(right_nullable.GetMethod("GetValueOrDefault", [])!);
        il.Stloc(right_value);
        var nullable = EmitStoreNullable(il, left_nullable, left_nullable_t, right_value);
        var endif_label = il.Br_S();

        // else: nullable = Nullable<nullable_t>();
        il.MarkLabel(else_label);
        il.Emit(OpCodes.Pop);
        il.Ldloca(nullable);
        il.Emit(OpCodes.Initobj, left_nullable);

        // endif: stack[top] = nullable;
        il.MarkLabel(endif_label);
        il.Ldloc(nullable);
    }

    public static void EmitCastViaObject(ILGenerator il, Type left_type, Type right_type_wrap_object)
    {
        if (Nullable.GetUnderlyingType(left_type) is { } value_type)
        {
            EmitNullableCastViaObject(il, left_type, value_type, right_type_wrap_object);
        }
        else if (right_type_wrap_object.IsValueType)
        {
            // if (stack[top] is DBNull)
            var else_label = EmitIfIsInstanceElseGoto<DBNull>(il);

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
            il.Emit(OpCodes.Unbox_Any, right_type_wrap_object);

            il.MarkLabel(endif_label);

            EmitCast(il, left_type, right_type_wrap_object);
        }
        else
        {
            EmitCast(il, left_type, typeof(object));
        }
    }

    public static void EmitCast(ILGenerator il, Type left_type, Type right_type)
    {
        if (left_type == right_type) return;
        if (left_type == typeof(string))
        {
            if (right_type.IsValueType)
            {
                if (Nullable.GetUnderlyingType(right_type) is { } right_nullable_t)
                {
                    var right_value = il.DeclareLocal(right_nullable_t);

                    // dup stack[top];
                    il.Emit(OpCodes.Dup);

                    // if (stack[top].HasValue)
                    il.Call(right_type.GetProperty("HasValue")!.GetGetMethod()!);
                    var else_label = il.IfTrueElseGoto();

                    // then: stack[top] = stack[top].Value.ToString();
                    il.Call(right_type.GetProperty("Value")!.GetGetMethod()!);
                    il.Stloc(right_value);
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
                var endif_label = EmitIfIsNullThenGoto(il);

                // if (stack[top] is DBNull)
                var else_label = EmitIfIsInstanceElseGoto<DBNull>(il);

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
                EmitNullableCast(il, left_type, left_nullable_t, right_type, right_nullable_t);
            }
            else if (right_type.IsValueType)
            {
                // right_value = (value_type)stack[top];
                var right_value = il.DeclareLocal(right_type);
                EmitCast(il, left_nullable_t, right_type);
                il.Stloc(right_value);

                // nullable = Nullable<nullable_t>(right_value);
                var nullable = EmitStoreNullable(il, left_type, left_nullable_t, right_value);

                // stack[top] = nullable;
                il.Ldloc(nullable);
            }
            else if (right_type == typeof(string))
            {
                // stack[top] = (left_type)(left_nullable_t)stack[top];
                EmitChangeType(il, left_nullable_t);
                EmitCast(il, left_type, left_nullable_t);
            }
            else
            {
                // if (stack[top] is string)
                var else_label = EmitIfIsInstanceElseGoto<string>(il);

                // then: stack[top] = (left_type)(string)stack[top];
                EmitCast(il, left_type, typeof(string));
                var endif_label = il.Br_S();

                // else: stack[top] = (left_type)stack[top];
                il.MarkLabel(else_label);
                EmitNullableCastViaObject(il, left_type, left_nullable_t, left_nullable_t);

                il.MarkLabel(endif_label);
            }
        }
        else if (left_type.IsValueType)
        {
            if (Nullable.GetUnderlyingType(right_type) is { } right_nullable_t)
            {
                // stack[top] = (from_type)stack[top].GetValueOrDefault();
                il.Call(right_type.GetMethod("GetValueOrDefault", [])!);
                EmitCast(il, left_type, right_nullable_t);
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
                EmitChangeType(il, left_type);
            }
            else
            {
                // if (stack[top] is null)
                var else1_label = EmitIfIsNullElseGoto(il);

                // then: stack[top] = 0;
                il.Emit(OpCodes.Pop);
                il.Ldc_I4(0);
                var endif_label = il.Br_S();

                // if (stack[top] is DBNull)
                il.MarkLabel(else1_label);
                var else2_label = EmitIfIsInstanceElseGoto<DBNull>(il);

                // then: stack[top] = 0;
                il.Emit(OpCodes.Pop);
                il.Ldc_I4(0);
                il.Br_S(endif_label);

                // if (stack[top] is string)
                il.MarkLabel(else2_label);
                var else3_label = EmitIfIsInstanceElseGoto<string>(il);

                // then: stack[top] = (left_type)(string)stack[top];
                EmitCast(il, left_type, typeof(string));
                il.Br_S(endif_label);

                // else: stack[top] = (left_type)stack[top];
                il.MarkLabel(else3_label);
                EmitChangeType(il, left_type);

                il.MarkLabel(endif_label);
            }
        }
        else if (!left_type.IsValueType && right_type.IsValueType)
        {
            // stack[top] = (object)stack[top];
            il.Emit(OpCodes.Box, right_type);
        }
        else
        {
            // stack[top] = (left_type)stack[top];
            il.Emit(OpCodes.Castclass, left_type);
        }
    }

    public static Type? EmitLoad<T>(ILGenerator il, string name)
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
}
