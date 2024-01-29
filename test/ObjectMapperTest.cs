using Mina.Extensions;
using Xunit;

namespace Mina.Test;

public class ObjectMapperTest
{
    public int PropInt1 { get; init; } = 1;
    public int PropInt2 { get; init; } = 2;
    public string PropStringAAA { get; init; } = "AAA";
    public string PropStringBBB { get; init; } = "BBB";

    public string FieldStr = "zzz";
    public int FieldInt = 1;

    [Fact]
    public void CreateGetMapperTest()
    {
        var receiver1 = new ObjectMapperTest();
        var receiver2 = new ObjectMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_int = ObjectMapper.CreateGetMapper<ObjectMapperTest, int>();
        var map_str = ObjectMapper.CreateGetMapper<ObjectMapperTest, string>();

        Assert.Equal(map_int["PropInt1"](receiver1), 1);
        Assert.Equal(map_int["PropInt2"](receiver1), 2);
        Assert.Equal(map_str["PropStringAAA"](receiver1), "AAA");
        Assert.Equal(map_str["PropStringBBB"](receiver1), "BBB");

        Assert.Equal(map_int["PropInt1"](receiver2), 10);
        Assert.Equal(map_int["PropInt2"](receiver2), 20);
        Assert.Equal(map_str["PropStringAAA"](receiver2), "XXX");
        Assert.Equal(map_str["PropStringBBB"](receiver2), "YYY");
    }

    [Fact]
    public void CreateGetMapperDynamicTest()
    {
        var receiver1 = new ObjectMapperTest();
        var receiver2 = new ObjectMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_dynamic = ObjectMapper.CreateGetMapper<ObjectMapperTest>();

        Assert.Equal(map_dynamic["PropInt1"](receiver1), 1);
        Assert.Equal(map_dynamic["PropInt2"](receiver1), 2);
        Assert.Equal(map_dynamic["PropStringAAA"](receiver1), "AAA");
        Assert.Equal(map_dynamic["PropStringBBB"](receiver1), "BBB");

        Assert.Equal(map_dynamic["PropInt1"](receiver2), 10);
        Assert.Equal(map_dynamic["PropInt2"](receiver2), 20);
        Assert.Equal(map_dynamic["PropStringAAA"](receiver2), "XXX");
        Assert.Equal(map_dynamic["PropStringBBB"](receiver2), "YYY");
    }

    [Fact]
    public void CreateSetMapperTest()
    {
        var receiver1 = new ObjectMapperTest();
        var receiver2 = new ObjectMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_int = ObjectMapper.CreateSetMapper<ObjectMapperTest, int>();
        var map_str = ObjectMapper.CreateSetMapper<ObjectMapperTest, string>();

        map_int["PropInt1"](receiver1, 2);
        map_int["PropInt2"](receiver1, 4);
        map_str["PropStringAAA"](receiver1, "aa");
        map_str["PropStringBBB"](receiver1, "bb");
        Assert.Equal(receiver1.PropInt1, 2);
        Assert.Equal(receiver1.PropInt2, 4);
        Assert.Equal(receiver1.PropStringAAA, "aa");
        Assert.Equal(receiver1.PropStringBBB, "bb");

        map_int["PropInt1"](receiver2, 20);
        map_int["PropInt2"](receiver2, 40);
        map_str["PropStringAAA"](receiver2, "a-a");
        map_str["PropStringBBB"](receiver2, "b-b");
        Assert.Equal(receiver2.PropInt1, 20);
        Assert.Equal(receiver2.PropInt2, 40);
        Assert.Equal(receiver2.PropStringAAA, "a-a");
        Assert.Equal(receiver2.PropStringBBB, "b-b");
    }

    [Fact]
    public void CreateSetMapperDynamicTest()
    {
        var receiver1 = new ObjectMapperTest();
        var receiver2 = new ObjectMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_dynamic = ObjectMapper.CreateSetMapper<ObjectMapperTest>();

        map_dynamic["PropInt1"](receiver1, 2);
        map_dynamic["PropInt2"](receiver1, 4);
        map_dynamic["PropStringAAA"](receiver1, "aa");
        map_dynamic["PropStringBBB"](receiver1, "bb");
        Assert.Equal(receiver1.PropInt1, 2);
        Assert.Equal(receiver1.PropInt2, 4);
        Assert.Equal(receiver1.PropStringAAA, "aa");
        Assert.Equal(receiver1.PropStringBBB, "bb");

        map_dynamic["PropInt1"](receiver2, 20);
        map_dynamic["PropInt2"](receiver2, 40);
        map_dynamic["PropStringAAA"](receiver2, "a-a");
        map_dynamic["PropStringBBB"](receiver2, "b-b");
        Assert.Equal(receiver2.PropInt1, 20);
        Assert.Equal(receiver2.PropInt2, 40);
        Assert.Equal(receiver2.PropStringAAA, "a-a");
        Assert.Equal(receiver2.PropStringBBB, "b-b");
    }

    [Fact]
    public void CreateFieldGetMapperTest()
    {
        var receiver = (StringAAA: "aa", Int1: 10);
        var map_str = ObjectMapper.CreateFieldGetMapper<(string StringAAA, int Int1), string>();
        var map_int = ObjectMapper.CreateFieldGetMapper<(string StringAAA, int Int1), int>();

        Assert.Equal(map_str["Item1"](receiver), "aa");
        Assert.Equal(map_int["Item2"](receiver), 10);
    }

    [Fact]
    public void CreateFieldGetMapperDynamicTest()
    {
        var receiver = (StringAAA: "aa", Int1: 10);
        var map_dynamic = ObjectMapper.CreateFieldGetMapper<(string StringAAA, int Int1)>();

        Assert.Equal(map_dynamic["Item1"](receiver), "aa");
        Assert.Equal(map_dynamic["Item2"](receiver), 10);
    }

    [Fact]
    public void CreateFieldSetMapperTest()
    {
        var receiver = new ObjectMapperTest();
        var map_str = ObjectMapper.CreateFieldSetMapper<ObjectMapperTest, string>();
        var map_int = ObjectMapper.CreateFieldSetMapper<ObjectMapperTest, int>();

        map_str["FieldStr"](receiver, "abc");
        map_int["FieldInt"](receiver, 123);
        Assert.Equal(receiver.FieldStr, "abc");
        Assert.Equal(receiver.FieldInt, 123);
    }

    [Fact]
    public void CreateFieldSetMapperDynamicTest()
    {
        var receiver = new ObjectMapperTest();
        var map_dynamic = ObjectMapper.CreateFieldSetMapper<ObjectMapperTest>();

        map_dynamic["FieldStr"](receiver, "abc");
        map_dynamic["FieldInt"](receiver, 123);
        Assert.Equal(receiver.FieldStr, "abc");
        Assert.Equal(receiver.FieldInt, 123);
    }
}
