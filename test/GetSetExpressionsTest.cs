using Mina.Reflection;
using Xunit;

namespace Mina.Test;

public class GetSetExpressionsTest
{
    public string Test { get; init; } = "xxx";
    public string Test2 { get; private set; } = "yyy";
    public int Test3 { get; private set; } = 0;

    public string FieldStr = "zzz";
    public int FieldInt = 1;

    public void Act() => Test2 = "aaa";

    public void Act_Str(string s) => Test2 = s;

    public void Act_Int(int n) => Test3 = n;

    public void Act_Int_Int(int a, int b) => Test3 = a + b;

    public string Fun_Str() => Test2;

    public int Fun_Int() => 123;

    public int Fun_Int_Int(int n) => n;

    public int Fun_Int_Int_Int(int a, int b) => a + b;

    [Fact]
    public void SetProperty()
    {
        var v = new GetSetExpressionsTest() { Test = "aaa" };
        var f = Expressions.SetProperty<GetSetExpressionsTest, string>("Test");
        f(v, "bbb");
        Assert.Equal(v.Test, "bbb");
    }

    [Fact]
    public void GetProperty()
    {
        var v = new GetSetExpressionsTest() { Test = "aaa" };
        var f = Expressions.GetProperty<GetSetExpressionsTest, string>("Test");
        var p = f(v);
        Assert.Equal(p, "aaa");
    }

    [Fact]
    public void GetAction()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetAction<GetSetExpressionsTest>("Act");
        f(v);
        Assert.Equal(v.Test2, "aaa");
    }

    [Fact]
    public void GetAction_Str()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetAction<GetSetExpressionsTest, string>("Act_Str");
        f(v, "bbb");
        Assert.Equal(v.Test2, "bbb");
    }

    [Fact]
    public void GetAction_StrInt()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetAction<GetSetExpressionsTest, int>("Act_Str");
        f(v, 123);
        Assert.Equal(v.Test2, "123");
    }

    [Fact]
    public void GetAction_IntLong()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetAction<GetSetExpressionsTest, long>("Act_Int");
        f(v, 123);
        Assert.Equal(v.Test3, 123);
    }

    [Fact]
    public void GetAction_IntStr()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetAction<GetSetExpressionsTest, string>("Act_Int");
        f(v, "123");
        Assert.Equal(v.Test3, 123);
    }

    [Fact]
    public void GetAction_Int_Int()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetAction<GetSetExpressionsTest, int, int>("Act_Int_Int");
        f(v, 123, 456);
        Assert.Equal(v.Test3, 579);
    }

    [Fact]
    public void GetFunction_Str()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetFunction<GetSetExpressionsTest, string>("Fun_Str");
        var p = f(v);
        Assert.Equal(p, "yyy");
    }

    [Fact]
    public void GetFunction_IntObject()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetFunction<GetSetExpressionsTest, object>("Fun_Int");
        var p = f(v);
        Assert.True(p is int a && a == 123);
    }

    [Fact]
    public void GetFunction_Int_Int()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetFunction<GetSetExpressionsTest, int, int>("Fun_Int_Int");
        var p = f(v, 123);
        Assert.Equal(p, 123);
    }

    [Fact]
    public void GetFunction_IntLong_IntLong()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetFunction<GetSetExpressionsTest, long, long>("Fun_Int_Int");
        var p = f(v, 123);
        Assert.Equal(p, 123);
    }

    [Fact]
    public void GetFunction_IntLong_IntObject()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetFunction<GetSetExpressionsTest, long, object>("Fun_Int_Int");
        var p = f(v, 123);
        Assert.True(p is int a && a == 123);
    }

    [Fact]
    public void GetFunction_Int_Int_Int()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetFunction<GetSetExpressionsTest, int, int, int>("Fun_Int_Int_Int");
        var p = f(v, 123, 456);
        Assert.True(p is int a && a == 579);
    }

    [Fact]
    public void GetField_Str()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetField<GetSetExpressionsTest, string>("FieldStr");
        var p = f(v);
        Assert.Equal(p, "zzz");
    }

    [Fact]
    public void GetField_Int()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetField<GetSetExpressionsTest, int>("FieldInt");
        var p = f(v);
        Assert.Equal(p, 1);
    }

    [Fact]
    public void GetField_IntLong()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.GetField<GetSetExpressionsTest, long>("FieldInt");
        var p = f(v);
        Assert.Equal(p, 1);
    }

    [Fact]
    public void SetField_Str()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.SetField<GetSetExpressionsTest, string>("FieldStr");
        f(v, "aaa");
        Assert.Equal(v.FieldStr, "aaa");
    }

    [Fact]
    public void SetField_Int()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.SetField<GetSetExpressionsTest, int>("FieldInt");
        f(v, 123);
        Assert.Equal(v.FieldInt, 123);
    }

    [Fact]
    public void SetField_IntLong()
    {
        var v = new GetSetExpressionsTest();
        var f = Expressions.SetField<GetSetExpressionsTest, long>("FieldInt");
        f(v, 123);
        Assert.Equal(v.FieldInt, 123);
    }
}
