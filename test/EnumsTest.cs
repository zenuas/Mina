using Mina.Attributes;
using Mina.Extension;
using System;
using Xunit;

namespace Mina.Test;

public class EnumsTest
{
    public enum TestEnum
    {
        Aaa,

        [Alias("Xxx")]
        Bbb,

        Ccc,
    }

    [Fact]
    public void GetAttributeOrDefaultTest()
    {
        var a = TestEnum.Aaa.GetAttributeOrDefault<AliasAttribute>();
        Assert.Equal(a, null);

        var b = TestEnum.Bbb.GetAttributeOrDefault<AliasAttribute>();
        Assert.Equal(b?.Name, "Xxx");

        var c = TestEnum.Aaa.GetAttributeOrDefault<AliasAttribute>();
        Assert.Equal(c, null);
    }

    [Fact]
    public void ParseTest()
    {
        var a = Enums.Parse<TestEnum>("Aaa");
        Assert.Equal(a, TestEnum.Aaa);

        var b = Enums.Parse<TestEnum>("Bbb");
        Assert.Equal(b, TestEnum.Bbb);

        var c = Enums.Parse<TestEnum>("Ccc");
        Assert.Equal(c, TestEnum.Ccc);

        var x = Enums.Parse<TestEnum>("Xxx");
        Assert.Equal(x, null);
    }

    [Fact]
    public void ParseWithAliasTest()
    {
        var a = Enums.ParseWithAlias<TestEnum>("Aaa");
        Assert.Equal(a, TestEnum.Aaa);

        var b = Enums.ParseWithAlias<TestEnum>("Bbb");
        Assert.Equal(b, TestEnum.Bbb);

        var c = Enums.ParseWithAlias<TestEnum>("Ccc");
        Assert.Equal(c, TestEnum.Ccc);

        var x = Enums.ParseWithAlias<TestEnum>("Xxx");
        Assert.Equal(x, TestEnum.Bbb);

        var y = Enums.ParseWithAlias<TestEnum>("Yyy");
        Assert.Equal(y, null);
    }

    [Flags]
    public enum TestBitEnum
    {
        Aaa = 1 << 0,

        Bbb = 1 << 1,

        Ccc = 1 << 2,

        BorC = Bbb | Ccc,
    }

    [Fact]
    public void HasFlagTest()
    {
        var a = TestBitEnum.Aaa;
        var b = TestBitEnum.Bbb;
        var c = TestBitEnum.Ccc;
        var borc = TestBitEnum.BorC;

        Assert.Equal(a.HasFlag(TestBitEnum.Aaa), true);
        Assert.Equal(a.HasFlag(TestBitEnum.Bbb), false);
        Assert.Equal(b.HasFlag(TestBitEnum.Aaa), false);
        Assert.Equal(b.HasFlag(TestBitEnum.Bbb), true);
        Assert.Equal(a.HasFlag(TestBitEnum.BorC), false);
        Assert.Equal(b.HasFlag(TestBitEnum.BorC), false);
        Assert.Equal(c.HasFlag(TestBitEnum.BorC), false);
        Assert.Equal(borc.HasFlag(TestBitEnum.BorC), true);
    }

    [Fact]
    public void HasBitTest()
    {
        var a = TestBitEnum.Aaa;
        var b = TestBitEnum.Bbb;
        var c = TestBitEnum.Ccc;
        var borc = TestBitEnum.BorC;

        Assert.Equal(a.HasBit(TestBitEnum.Aaa), true);
        Assert.Equal(a.HasBit(TestBitEnum.Bbb), false);
        Assert.Equal(b.HasBit(TestBitEnum.Aaa), false);
        Assert.Equal(b.HasBit(TestBitEnum.Bbb), true);
        Assert.Equal(a.HasBit(TestBitEnum.BorC), false);
        Assert.Equal(b.HasBit(TestBitEnum.BorC), true);
        Assert.Equal(c.HasBit(TestBitEnum.BorC), true);
        Assert.Equal(borc.HasBit(TestBitEnum.BorC), true);
    }
}
