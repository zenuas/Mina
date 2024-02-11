using Mina.Reflections;
using System;
using System.Reflection.Emit;
using Xunit;

namespace Mina.Test;

public class EmitCastTest
{
    public class Data
    {
        public int Int { get; set; } = 999;
        public int? Inta { get; set; } = 999;
        public long Long { get; set; } = 999;
        public long? Longa { get; set; } = 999;
        public DateTime Date { get; set; } = DateTime.Parse("1950/12/31");
        public DateTime? Datea { get; set; } = DateTime.Parse("1950/12/31");
        public string String { get; set; } = "dummy";
        public string? Stringa { get; set; } = "dummy";
    }

    [Fact]
    public void Int_Int()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(int), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int>>();

        var x = new Data();
        int n = 123;
        f(x, n);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Int_Inta()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(int?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = 123;
        f(x, n);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Int_IntaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(int?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = null;
        f(x, n);
        Assert.Equal(x.Int, 0);
    }

    [Fact]
    public void Int_Long()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(long), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long>>();

        var x = new Data();
        long n = 123L;
        f(x, n);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Int_Longa()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(long?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = 123;
        f(x, n);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Int_LongaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(long?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = null;
        f(x, n);
        Assert.Equal(x.Int, 0);
    }

    [Fact]
    public void Int_ObjectInt()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123;
        f(x, n);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Int_ObjectLong()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123L;
        f(x, n);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Int_ObjectNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.Int, 0);
    }

    [Fact]
    public void Int_ObjectDBNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.Int, 0);
    }

    [Fact]
    public void Int_Object0()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 0;
        f(x, n);
        Assert.Equal(x.Int, 0);
    }

    [Fact]
    public void Int_ObjectString()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "123";
        f(x, n);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Int_String()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int), typeof(string), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Int))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string s = "123";
        f(x, s);
        Assert.Equal(x.Int, 123);
    }

    [Fact]
    public void Inta_Int()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(int), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int>>();

        var x = new Data();
        int n = 123;
        f(x, n);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Inta_Inta()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(int?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = 123;
        f(x, n);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Inta_IntaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(int?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = null;
        f(x, n);
        Assert.Equal(x.Inta, null);
    }

    [Fact]
    public void Inta_Long()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(long), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long>>();

        var x = new Data();
        long n = 123L;
        f(x, n);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Inta_Longa()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(long?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = 123;
        f(x, n);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Inta_LongaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(long?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(long?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, long?>>();

        var x = new Data();
        long? n = null;
        f(x, n);
        Assert.Equal(x.Inta, null);
    }

    [Fact]
    public void Inta_ObjectInt()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123;
        f(x, n);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Inta_ObjectLong()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123L;
        f(x, n);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Inta_ObjectNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.Inta, null);
    }

    [Fact]
    public void Inta_ObjectDBNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.Inta, null);
    }

    [Fact]
    public void Inta_Object0()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 0;
        f(x, n);
        Assert.Equal(x.Inta, 0);
    }

    [Fact]
    public void Inta_ObjectString()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "123";
        f(x, n);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Inta_String()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(int?), typeof(string), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Inta))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string s = "123";
        f(x, s);
        Assert.Equal(x.Inta, 123);
    }

    [Fact]
    public void Date_Date()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(DateTime), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime>>();

        var x = new Data();
        DateTime n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Date, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Date_Dateatea()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(DateTime?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Date, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Date_DateateaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(DateTime?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = null;
        f(x, n);
        Assert.Equal(x.Date, default);
    }

    [Fact]
    public void Date_ObjectDate()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Date, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Date_ObjectNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.Date, default);
    }

    [Fact]
    public void Date_ObjectDBNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.Date, default);
    }

    [Fact]
    public void Date_ObjectString()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.Date, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Date_String()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime), typeof(string), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Date))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.Date, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Datea_Date()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(DateTime), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime>>();

        var x = new Data();
        DateTime n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Datea, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Datea_Dateatea()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(DateTime?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Datea, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Datea_DateateaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(DateTime?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(DateTime?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, DateTime?>>();

        var x = new Data();
        DateTime? n = null;
        f(x, n);
        Assert.Equal(x.Datea, default);
    }

    [Fact]
    public void Datea_ObjectDate()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DateTime.Parse("2000/01/01");
        f(x, n);
        Assert.Equal(x.Datea, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Datea_ObjectNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.Datea, null);
    }

    [Fact]
    public void Datea_ObjectDBNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.Datea, null);
    }

    [Fact]
    public void Datea_ObjectString()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.Datea, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void Datea_String()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(DateTime?), typeof(string), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.Datea))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string n = "2000/01/01";
        f(x, n);
        Assert.Equal(x.Datea, DateTime.Parse("2000/01/01"));
    }

    [Fact]
    public void String_String()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(string), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string>>();

        var x = new Data();
        string n = "abc";
        f(x, n);
        Assert.Equal(x.String, "abc");
    }

    [Fact]
    public void String_Stringtringa()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(string), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string?>>();

        var x = new Data();
        string? n = "abc";
        f(x, n);
        Assert.Equal(x.String, "abc");
    }

    [Fact]
    public void String_StringtringaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(string)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(string), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, string?>>();

        var x = new Data();
        string? n = null;
        f(x, n);
        Assert.Equal(x.String, null);
    }

    [Fact]
    public void String_Int()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(int), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int>>();

        var x = new Data();
        int n = 123;
        f(x, n);
        Assert.Equal(x.String, "123");
    }

    [Fact]
    public void String_Inta()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(int?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = 123;
        f(x, n);
        Assert.Equal(x.String, "123");
    }

    [Fact]
    public void String_IntaNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(int?)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(int?), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, int?>>();

        var x = new Data();
        int? n = null;
        f(x, n);
        Assert.Equal(x.String, null);
    }

    [Fact]
    public void String_ObjectNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = null;
        f(x, n);
        Assert.Equal(x.String, null);
    }

    [Fact]
    public void String_ObjectDBNull()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = DBNull.Value;
        f(x, n);
        Assert.Equal(x.String, null);
    }

    [Fact]
    public void String_ObjectString()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = "abc";
        f(x, n);
        Assert.Equal(x.String, "abc");
    }

    [Fact]
    public void String_ObjectInt()
    {
        var ilmethod = new DynamicMethod("", null, [typeof(Data), typeof(object)]);
        var il = ilmethod.GetILGenerator();
        il.Ldarg(0);
        il.LdargCast(typeof(string), typeof(object), 1);
        il.Call(typeof(Data).GetProperty(nameof(Data.String))!.GetSetMethod()!);
        il.Emit(OpCodes.Ret);
        var f = ilmethod.CreateDelegate<Action<Data, object?>>();

        var x = new Data();
        object? n = 123;
        f(x, n);
        Assert.Equal(x.String, "123");
    }
}
