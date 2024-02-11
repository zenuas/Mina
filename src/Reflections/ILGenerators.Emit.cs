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

    public static void Call(this ILGenerator il, MethodInfo method) => il.EmitCall(method.IsFinal || !method.IsVirtual ? OpCodes.Call : OpCodes.Callvirt, method, null);
}
