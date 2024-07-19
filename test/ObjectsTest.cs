using Mina.Extension;
using System;
using Xunit;

namespace Mina.Test;

public class ObjectsTest
{
    public enum TestEnum
    {
        Aaa,
        Bbb,
        Ccc,
        Xxx,
    }

    [Fact]
    public void InTest()
    {
        Assert.Equal(1.In(1, 2, 3), true);
        Assert.Equal(2.In(1, 2, 3), true);
        Assert.Equal(3.In(1, 2, 3), true);
        Assert.Equal(4.In(1, 2, 3), false);

        Assert.Equal("abc".In("abc", "bcd", "cde"), true);
        Assert.Equal("bcd".In("abc", "bcd", "cde"), true);
        Assert.Equal("cde".In("abc", "bcd", "cde"), true);
        Assert.Equal("def".In("abc", "bcd", "cde"), false);

        Assert.Equal((1, "a").In((1, "a"), (2, "b"), (3, "c")), true);
        Assert.Equal((2, "b").In((1, "a"), (2, "b"), (3, "c")), true);
        Assert.Equal((3, "c").In((1, "a"), (2, "b"), (3, "c")), true);
        Assert.Equal((4, "d").In((1, "a"), (2, "b"), (3, "c")), false);
        Assert.Equal((1, "b").In((1, "a"), (2, "b"), (3, "c")), false);
    }

    [Fact]
    public void InClassTest()
    {
        Assert.Equal("abc".InClass("abc", "bcd", "cde"), true);
        Assert.Equal("bcd".InClass("abc", "bcd", "cde"), true);
        Assert.Equal("cde".InClass("abc", "bcd", "cde"), true);
        Assert.Equal("def".InClass("abc", "bcd", "cde"), false);

        Assert.Equal(Tuple.Create(1, "a").InClass(Tuple.Create(1, "a"), Tuple.Create(2, "b"), Tuple.Create(3, "c")), true);
        Assert.Equal(Tuple.Create(2, "b").InClass(Tuple.Create(1, "a"), Tuple.Create(2, "b"), Tuple.Create(3, "c")), true);
        Assert.Equal(Tuple.Create(3, "c").InClass(Tuple.Create(1, "a"), Tuple.Create(2, "b"), Tuple.Create(3, "c")), true);
        Assert.Equal(Tuple.Create(4, "d").InClass(Tuple.Create(1, "a"), Tuple.Create(2, "b"), Tuple.Create(3, "c")), false);
        Assert.Equal(Tuple.Create(1, "b").InClass(Tuple.Create(1, "a"), Tuple.Create(2, "b"), Tuple.Create(3, "c")), false);
    }

    [Fact]
    public void InStructTest()
    {
        Assert.Equal(1.InStruct(1, 2, 3), true);
        Assert.Equal(2.InStruct(1, 2, 3), true);
        Assert.Equal(3.InStruct(1, 2, 3), true);
        Assert.Equal(4.InStruct(1, 2, 3), false);

        Assert.Equal((1, "a").InStruct((1, "a"), (2, "b"), (3, "c")), true);
        Assert.Equal((2, "b").InStruct((1, "a"), (2, "b"), (3, "c")), true);
        Assert.Equal((3, "c").InStruct((1, "a"), (2, "b"), (3, "c")), true);
        Assert.Equal((4, "d").InStruct((1, "a"), (2, "b"), (3, "c")), false);
        Assert.Equal((1, "b").InStruct((1, "a"), (2, "b"), (3, "c")), false);

        Assert.Equal(TestEnum.Aaa.InStruct(TestEnum.Aaa, TestEnum.Bbb, TestEnum.Ccc), true);
        Assert.Equal(TestEnum.Bbb.InStruct(TestEnum.Aaa, TestEnum.Bbb, TestEnum.Ccc), true);
        Assert.Equal(TestEnum.Ccc.InStruct(TestEnum.Aaa, TestEnum.Bbb, TestEnum.Ccc), true);
        Assert.Equal(TestEnum.Xxx.InStruct(TestEnum.Aaa, TestEnum.Bbb, TestEnum.Ccc), false);
    }

    [Fact]
    public void TryTest()
    {
        String? ok = "ok";
        String? ng = null;

        Assert.Equal(ok.Try(), "ok");
        Assert.Equal(ok.Try().GetType(), typeof(string));
        Assert.Throws<Exception>(() => ng.Try());

        int? ok2 = 100;
        int? ng2 = null;

        Assert.Equal(ok2.Try(), 100);
        Assert.Equal(ok2.Try().GetType(), typeof(int));
        Assert.Throws<Exception>(() => ng2.Try());
    }
}
