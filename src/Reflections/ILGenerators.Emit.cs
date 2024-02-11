﻿using Mina.Extensions;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Mina.Reflections;

public static partial class ILGenerators
{
    public static void Ldarg(this ILGenerator il, int argn)
    {
        switch (argn)
        {
            case 0: il.Emit(OpCodes.Ldarg_0); break;
            case 1: il.Emit(OpCodes.Ldarg_1); break;
            case 2: il.Emit(OpCodes.Ldarg_2); break;
            case 3: il.Emit(OpCodes.Ldarg_3); break;
            default: il.Emit(argn <= 255 ? OpCodes.Ldarg_S : OpCodes.Ldarg, argn); break;
        }
    }

    public static void Ldarga(this ILGenerator il, int argn) => il.Emit(argn <= 255 ? OpCodes.Ldarga_S : OpCodes.Ldarga, argn);

    public static void Ldloc(this ILGenerator il, int argn)
    {
        switch (argn)
        {
            case 0: il.Emit(OpCodes.Ldloc_0); break;
            case 1: il.Emit(OpCodes.Ldloc_1); break;
            case 2: il.Emit(OpCodes.Ldloc_2); break;
            case 3: il.Emit(OpCodes.Ldloc_3); break;
            default: il.Emit(argn <= 255 ? OpCodes.Ldloc_S : OpCodes.Ldloc, argn); break;
        }
    }

    public static void Ldloc(this ILGenerator il, LocalBuilder local) => il.Emit(OpCodes.Ldloc, local);

    public static void Ldloca(this ILGenerator il, int argn) => il.Emit(argn <= 255 ? OpCodes.Ldloca_S : OpCodes.Ldloca, argn);

    public static void Ldloca(this ILGenerator il, LocalBuilder local) => il.Emit(OpCodes.Ldloca, local);

    public static void Starg(this ILGenerator il, int argn) => il.Emit(argn <= 255 ? OpCodes.Starg_S : OpCodes.Starg, argn);

    public static void Stloc(this ILGenerator il, int argn)
    {
        switch (argn)
        {
            case 0: il.Emit(OpCodes.Stloc_0); break;
            case 1: il.Emit(OpCodes.Stloc_1); break;
            case 2: il.Emit(OpCodes.Stloc_2); break;
            case 3: il.Emit(OpCodes.Stloc_3); break;
            default: il.Emit(argn <= 255 ? OpCodes.Stloc_S : OpCodes.Stloc, argn); break;
        }
    }

    public static void Stloc(this ILGenerator il, LocalBuilder local) => il.Emit(OpCodes.Stloc, local);

    public static void Ldc_I4(this ILGenerator il, int n)
    {
        switch (n)
        {
            case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
            case 0: il.Emit(OpCodes.Ldc_I4_0); break;
            case 1: il.Emit(OpCodes.Ldc_I4_1); break;
            case 2: il.Emit(OpCodes.Ldc_I4_2); break;
            case 3: il.Emit(OpCodes.Ldc_I4_3); break;
            case 4: il.Emit(OpCodes.Ldc_I4_4); break;
            case 5: il.Emit(OpCodes.Ldc_I4_5); break;
            case 6: il.Emit(OpCodes.Ldc_I4_6); break;
            case 7: il.Emit(OpCodes.Ldc_I4_7); break;
            case 8: il.Emit(OpCodes.Ldc_I4_8); break;
            default: il.Emit(n >= 0 && n <= 255 ? OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, n); break;
        }
    }

    public static void Ldstr(this ILGenerator il, string s) => il.Emit(OpCodes.Ldstr, s);

    public static void Ldtoken<T>(this ILGenerator il) => il.Emit(OpCodes.Ldtoken, typeof(T));
    public static void Ldtoken(this ILGenerator il, Type t) => il.Emit(OpCodes.Ldtoken, t);

    public static void Box<T>(this ILGenerator il) => il.Emit(OpCodes.Box, typeof(T));
    public static void Box(this ILGenerator il, Type t) => il.Emit(OpCodes.Box, t);
    public static void Unbox_Any<T>(this ILGenerator il) => il.Emit(OpCodes.Unbox_Any, typeof(T));
    public static void Unbox_Any(this ILGenerator il, Type t) => il.Emit(OpCodes.Unbox_Any, t);
    public static void Castclass<T>(this ILGenerator il) => il.Emit(OpCodes.Castclass, typeof(T));
    public static void Castclass(this ILGenerator il, Type t) => il.Emit(OpCodes.Castclass, t);

    public static void Ldfld(this ILGenerator il, FieldInfo field) => il.Emit(OpCodes.Ldfld, field);
    public static void Ldflda(this ILGenerator il, FieldInfo field) => il.Emit(OpCodes.Ldflda, field);
    public static void Stfld(this ILGenerator il, FieldInfo field) => il.Emit(OpCodes.Stfld, field);

    public static void Call(this ILGenerator il, MethodInfo method) => il.EmitCall(method.IsFinal || !method.IsVirtual ? OpCodes.Call : OpCodes.Callvirt, method, null);
    public static void Call(this ILGenerator il, ConstructorInfo ctor) => il.Emit(OpCodes.Call, ctor);
    public static void Newobj(this ILGenerator il, ConstructorInfo ctor) => il.Emit(OpCodes.Newobj, ctor);
    public static void Initobj<T>(this ILGenerator il) => il.Emit(OpCodes.Initobj, typeof(T));
    public static void Initobj(this ILGenerator il, Type t) => il.Emit(OpCodes.Initobj, t);

    public static void Isinst<T>(this ILGenerator il) => il.Emit(OpCodes.Isinst, typeof(T));
    public static void Isinst(this ILGenerator il, Type t) => il.Emit(OpCodes.Isinst, t);

    public static Label Br_S(this ILGenerator il, Label? goto_label = null) => (goto_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Br_S, x));
    public static Label Br(this ILGenerator il, Label? goto_label = null) => (goto_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Br, x));

    public static Label IfTrueThenGoto_S(this ILGenerator il, Label? then_label = null) => (then_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brtrue_S, x));
    public static Label IfFalseThenGoto_S(this ILGenerator il, Label? then_label = null) => (then_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brfalse_S, x));
    public static Label IfTrueElseGoto_S(this ILGenerator il, Label? else_label = null) => (else_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brfalse_S, x));
    public static Label IfFalseElseGoto_S(this ILGenerator il, Label? else_label = null) => (else_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brtrue_S, x));

    public static Label IfTrueThenGoto(this ILGenerator il, Label? then_label = null) => (then_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brtrue, x));
    public static Label IfFalseThenGoto(this ILGenerator il, Label? then_label = null) => (then_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brfalse, x));
    public static Label IfTrueElseGoto(this ILGenerator il, Label? else_label = null) => (else_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brfalse, x));
    public static Label IfFalseElseGoto(this ILGenerator il, Label? else_label = null) => (else_label ?? il.DefineLabel()).Return(x => il.Emit(OpCodes.Brtrue, x));
}
