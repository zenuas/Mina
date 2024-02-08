using Mina.Extensions;
using System;
using System.Reflection.Emit;
using Xunit;

namespace Mina.Test;

public class EmitCastTest
{
    public class Data
    {
        public int V { get; set; } = 999;
        public int? Vn { get; set; } = 999;
        public long W { get; set; } = 999;
        public long? Wn { get; set; } = 999;
        public DateTime D { get; set; } = DateTime.Parse("1950/12/31");
        public DateTime? Dn { get; set; } = DateTime.Parse("1950/12/31");
        public string S { get; set; } = "dummy";
        public string? Sn { get; set; } = "dummy";
    }

    [Fact]
    public void V_V()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(int));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int>>();

        var x = new Data();
        int n = 123;
        f(x, n);
        Assert.Equal(x.V, 123);
    }

    [Fact]
    public void V_Vn()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(int), typeof(int?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = 123;
        f(x, n);
        Assert.Equal(x.V, 123);
    }

    [Fact]
    public void V_Vx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(int), typeof(int?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = null;
        f(x, n);
        Assert.Equal(x.V, 0);
    }

    [Fact]
    public void V_W()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(long));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long>>();

        var x = new Data();
        long n = 123L;
        f(x, n);
        Assert.Equal(x.V, 123);
    }

    [Fact]
    public void V_Wn()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(int), typeof(long?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = 123;
        f(x, n);
        Assert.Equal(x.V, 123);
    }

    [Fact]
    public void V_Wx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(int), typeof(long?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = null;
        f(x, n);
        Assert.Equal(x.V, 0);
    }

    [Fact]
    public void V_Tv()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123;
        f(x, n);
        Assert.Equal(x.V, 123);
    }

    [Fact]
    public void V_Tw()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123L;
        _ = Assert.Throws<InvalidCastException>(() => f(x, n));
    }

    [Fact]
    public void V_Tx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.V, 0);
    }

    [Fact]
    public void V_To()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.V, 0);
    }

    [Fact]
    public void V_T0()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 0;
        f(x, n);
        Assert.Equal(x.V, 0);
    }

    [Fact]
    public void V_Ts()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "123";
        f(x, n);
        Assert.Equal(x.V, 123);
    }

    [Fact]
    public void V_S()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int), typeof(string));
        Expressions.EmitCall(il, typeof(Data).GetProperty("V")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string s = "123";
        f(x, s);
        Assert.Equal(x.V, 123);
    }

    [Fact]
    public void Vn_V()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(int));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int>>();

        var x = new Data();
        int n = 123;
        f(x, n);
        Assert.Equal(x.Vn, 123);
    }

    [Fact]
    public void Vn_Vn()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(int?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = 123;
        f(x, n);
        Assert.Equal(x.Vn, 123);
    }

    [Fact]
    public void Vn_Vx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(int?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = null;
        f(x, n);
        Assert.Equal(x.Vn, null);
    }

    [Fact]
    public void Vn_W()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(long));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long>>();

        var x = new Data();
        long n = 123L;
        f(x, n);
        Assert.Equal(x.Vn, 123);
    }

    [Fact]
    public void Vn_Wn()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(int?), typeof(long?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = 123;
        f(x, n);
        Assert.Equal(x.Vn, 123);
    }

    [Fact]
    public void Vn_Wx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(int?), typeof(long?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = null;
        f(x, n);
        Assert.Equal(x.Vn, null);
    }

    [Fact]
    public void Vn_Tv()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123;
        f(x, n);
        Assert.Equal(x.Vn, 123);
    }

    [Fact]
    public void Vn_Tw()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123L;
        f(x, n);
        Assert.Equal(x.Vn, null);
    }

    [Fact]
    public void Vn_Tx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.Vn, null);
    }

    [Fact]
    public void Vn_To()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.Vn, null);
    }

    [Fact]
    public void Vn_T0()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 0;
        f(x, n);
        Assert.Equal(x.Vn, 0);
    }

    [Fact]
    public void Vn_Ts()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "123";
        f(x, n);
        Assert.Equal(x.Vn, 123);
    }

    [Fact]
    public void Vn_S()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(int?), typeof(string));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Vn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string s = "123";
        f(x, s);
        Assert.Equal(x.Vn, 123);
    }

    [Fact]
    public void D_D()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(DateTime));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime>>();

        var x = new Data();
        DateTime n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.D, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void D_Dn()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(DateTime?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.D, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void D_Dx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarga_S, 1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(DateTime?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = null;
        f(x, n);
        Assert.Equal(x.D, default);
    }

    [Fact]
    public void D_Td()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.D, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void D_Tx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.D, default);
    }

    [Fact]
    public void D_To()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.D, default);
    }

    [Fact]
    public void D_Ts()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.D, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void D_S()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime), typeof(string));
        Expressions.EmitCall(il, typeof(Data).GetProperty("D")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.D, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Dn_D()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(DateTime));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime>>();

        var x = new Data();
        DateTime n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Dn, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Dn_Dn()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(DateTime?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Dn, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Dn_Dx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(DateTime?));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = null;
        f(x, n);
        Assert.Equal(x.Dn, default);
    }

    [Fact]
    public void Dn_Td()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Dn, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Dn_Tx()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.Dn, null);
    }

    [Fact]
    public void Dn_To()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.Dn, null);
    }

    [Fact]
    public void Dn_Ts()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(object));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.Dn, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Dn_S()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        Expressions.EmitCast(il, typeof(DateTime?), typeof(string));
        Expressions.EmitCall(il, typeof(Data).GetProperty("Dn")!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.Dn, DateTime.Parse("2000/01/01"));
    }
}
