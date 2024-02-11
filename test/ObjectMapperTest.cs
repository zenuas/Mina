using Mina.Extensions;
using System.Collections.Generic;
using Xunit;

namespace Mina.Test;

public class ObjectMapperTest
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

    public struct StructData
    {
        public int Prop;
        public string Method;
        public long Field;
    }

    public record class RecordData(int Prop, string Method, long Field);

    public record struct RecordStructData(int Prop, string Method, long Field);

    [Fact]
    public void ToProp()
    {
        var v = new ObjectMapperTest() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapperTest, PropData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d.Prop, 123);
        Assert.Equal(d.Method, "test");
        Assert.Equal(d.Field, 456);
    }

    [Fact]
    public void ToMethod()
    {
        var v = new ObjectMapperTest() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapperTest, MethodData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d._Prop, 123);
        Assert.Equal(d._Method, "test");
        Assert.Equal(d._Field, 456);
    }

    [Fact]
    public void ToMethodVoid()
    {
        var v = new ObjectMapperTest() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapperTest, MethodVoidData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d._Prop, 123);
        Assert.Equal(d._Method, "test");
        Assert.Equal(d._Field, 456);
    }

    [Fact]
    public void ToMethodVoid2()
    {
        var v = new ObjectMapperTest() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapperTest, MethodVoidData>(new Dictionary<string, string>() {
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
    public void ToField()
    {
        var v = new ObjectMapperTest() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapperTest, FieldData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d.Prop, 123);
        Assert.Equal(d.Method, "test");
        Assert.Equal(d.Field, 456);
    }

    [Fact]
    public void ToStruct()
    {
        var v = new ObjectMapperTest() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapperTest, StructData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d.Prop, 123);
        Assert.Equal(d.Method, "test");
        Assert.Equal(d.Field, 456);
    }

    [Fact]
    public void ToRecord()
    {
        var v = new ObjectMapperTest() { Prop = 123, Field = 456 };
        var f = ObjectMapper.CreateMapper<ObjectMapperTest, RecordData>(["Prop", "Method", "Field"]);
        var d = f(v);
        Assert.Equal(d.Prop, 123);
        Assert.Equal(d.Method, "test");
        Assert.Equal(d.Field, 456);
    }
}
