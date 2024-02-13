using Mina.Mapper;
using Xunit;

namespace Mina.Test;

public class GetSetMapperTest
{
    public int PropInt1 { get; init; } = 1;
    public int PropInt2 { get; init; } = 2;
    public string PropStringAAA { get; init; } = "AAA";
    public string PropStringBBB { get; init; } = "BBB";

    public string FieldStr = "zzz";
    public int FieldInt = 1;

    [Fact]
    public void GetMap()
    {
        var receiver1 = new GetSetMapperTest();
        var receiver2 = new GetSetMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_int = InstanceMapper.CreateGetMapper<GetSetMapperTest, int>();
        var map_str = InstanceMapper.CreateGetMapper<GetSetMapperTest, string>();

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
    public void DynamicGetMap()
    {
        var receiver1 = new GetSetMapperTest();
        var receiver2 = new GetSetMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_dynamic = InstanceMapper.CreateGetMapper<GetSetMapperTest>();

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
    public void SetMap()
    {
        var receiver1 = new GetSetMapperTest();
        var receiver2 = new GetSetMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_int = InstanceMapper.CreateSetMapper<GetSetMapperTest, int>();
        var map_str = InstanceMapper.CreateSetMapper<GetSetMapperTest, string>();

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
    public void DynamicSetMap()
    {
        var receiver1 = new GetSetMapperTest();
        var receiver2 = new GetSetMapperTest() { PropInt1 = 10, PropInt2 = 20, PropStringAAA = "XXX", PropStringBBB = "YYY" };
        var map_dynamic = InstanceMapper.CreateSetMapper<GetSetMapperTest>();

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
    public void TupleGetMap()
    {
        var receiver = (StringAAA: "aa", Int1: 10);
        var map_str = InstanceMapper.CreateFieldGetMapper<(string StringAAA, int Int1), string>();
        var map_int = InstanceMapper.CreateFieldGetMapper<(string StringAAA, int Int1), int>();

        Assert.Equal(map_str["Item1"](receiver), "aa");
        Assert.Equal(map_int["Item2"](receiver), 10);
    }

    [Fact]
    public void TupleDynamicGetMap()
    {
        var receiver = (StringAAA: "aa", Int1: 10);
        var map_dynamic = InstanceMapper.CreateFieldGetMapper<(string StringAAA, int Int1)>();

        Assert.Equal(map_dynamic["Item1"](receiver), "aa");
        Assert.Equal(map_dynamic["Item2"](receiver), 10);
    }

    [Fact]
    public void FieldGetMap()
    {
        var receiver = new GetSetMapperTest() { FieldStr = "abc", FieldInt = 123 };
        var map_str = InstanceMapper.CreateFieldGetMapper<GetSetMapperTest, string>();
        var map_int = InstanceMapper.CreateFieldGetMapper<GetSetMapperTest, int>();

        Assert.Equal(map_str["FieldStr"](receiver), "abc");
        Assert.Equal(map_int["FieldInt"](receiver), 123);
    }

    [Fact]
    public void FieldDynamicGetMap()
    {
        var receiver = new GetSetMapperTest() { FieldStr = "abc", FieldInt = 123 };
        var map_dynamic = InstanceMapper.CreateFieldGetMapper<GetSetMapperTest>();

        Assert.Equal(map_dynamic["FieldStr"](receiver), "abc");
        Assert.Equal(map_dynamic["FieldInt"](receiver), 123);
    }

    [Fact]
    public void FieldSetMap()
    {
        var receiver = new GetSetMapperTest();
        var map_str = InstanceMapper.CreateFieldSetMapper<GetSetMapperTest, string>();
        var map_int = InstanceMapper.CreateFieldSetMapper<GetSetMapperTest, int>();

        map_str["FieldStr"](receiver, "abc");
        map_int["FieldInt"](receiver, 123);
        Assert.Equal(receiver.FieldStr, "abc");
        Assert.Equal(receiver.FieldInt, 123);
    }

    [Fact]
    public void FieldDynamicSetMap()
    {
        var receiver = new GetSetMapperTest();
        var map_dynamic = InstanceMapper.CreateFieldSetMapper<GetSetMapperTest>();

        map_dynamic["FieldStr"](receiver, "abc");
        map_dynamic["FieldInt"](receiver, 123);
        Assert.Equal(receiver.FieldStr, "abc");
        Assert.Equal(receiver.FieldInt, 123);
    }
}
