using Mina.Extensions;
using System.Collections.Generic;
using Xunit;

namespace Mina.Test;

public class ObjectMapper2Test
{
    public int Prop { get; set; } = 1;
    public string Method() => "test";
    public long Field = 2;

    public class PropData
    {
        public int Prop { get; set; }
        public string Method { get; set; } = "";
        public long Field { get; set; }
    }

    public class MethodData
    {
        public int _Prop;
        public string _Method = "";
        public long _Field;

        public int Prop(int x) => _Prop = x;
        public string Method(string x) => _Method = x;
        public long Field(long x) => _Field = x;
    }

    public class MethodVoidData
    {
        public int _Prop;
        public string _Method = "";
        public long _Field;

        public void Prop(int x) => _Prop = x;
        public void Method(string x) => _Method = x;
        public void Field(long x) => _Field = x;
    }

    public class FieldData
    {
        public int Prop;
        public string Method = "";
        public long Field;
    }

    [Fact]
    public void CreateMapperToProp()
    {
        var v = new ObjectMapper2Test() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapper2Test, PropData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d.Prop, 123);
        Assert.Equal(d.Method, "test");
        Assert.Equal(d.Field, 456);
    }

    [Fact]
    public void CreateMapperToMethod()
    {
        var v = new ObjectMapper2Test() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapper2Test, MethodData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d._Prop, 123);
        Assert.Equal(d._Method, "test");
        Assert.Equal(d._Field, 456);
    }

    [Fact]
    public void CreateMapperToMethodVoid()
    {
        var v = new ObjectMapper2Test() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapper2Test, MethodVoidData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d._Prop, 123);
        Assert.Equal(d._Method, "test");
        Assert.Equal(d._Field, 456);
    }

    [Fact]
    public void CreateMapperToMethodVoid2()
    {
        var v = new ObjectMapper2Test() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapper2Test, MethodVoidData>(new Dictionary<string, string>() {
            { "Prop", "_Field" },
            { "Method", "_Method" },
            { "Field", "_Prop" }
        });
        var d = f(v);
        Assert.Equal(d._Prop, 456);
        Assert.Equal(d._Method, "test");
        Assert.Equal(d._Field, 123);
    }

    [Fact]
    public void CreateMapperToField()
    {
        var v = new ObjectMapper2Test() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapper2Test, FieldData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d.Prop, 123);
        Assert.Equal(d.Method, "test");
        Assert.Equal(d.Field, 456);
    }
}
