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
        Assert.Throws<NullReferenceException>(() => ng.Try<string, NullReferenceException>());

        int? ok2 = 100;
        int? ng2 = null;

        Assert.Equal(ok2.Try(), 100);
        Assert.Equal(ok2.Try().GetType(), typeof(int));
        Assert.Throws<Exception>(() => ng2.Try());
        Assert.Throws<NullReferenceException>(() => ng2.Try<int, NullReferenceException>());
    }

    public struct TestStruct1
    {
        public byte Byte = 0;

        public TestStruct1()
        {
        }
    }

    public struct TestStruct2
    {
        public short Short = 0;

        public TestStruct2()
        {
        }
    }

    public struct TestStruct3
    {
        public byte Byte = 0;
        public short Short = 0;

        public TestStruct3()
        {
        }
    }

    public class TestClass
    {
        public byte Byte { get; init; }
    }

    [Fact]
    public void SizeOfTest()
    {
        sbyte sb = 0;
        byte ub = 0;
        Assert.Equal(sb.SizeOf(), sizeof(sbyte));
        Assert.Equal(ub.SizeOf(), sizeof(byte));

        short ss = 0;
        ushort us = 0;
        Assert.Equal(ss.SizeOf(), sizeof(short));
        Assert.Equal(us.SizeOf(), sizeof(ushort));

        int si = 0;
        uint ui = 0;
        Assert.Equal(si.SizeOf(), sizeof(int));
        Assert.Equal(ui.SizeOf(), sizeof(uint));

        long sl = 0;
        ulong ul = 0;
        Assert.Equal(sl.SizeOf(), sizeof(long));
        Assert.Equal(ul.SizeOf(), sizeof(ulong));

        float fl = 0;
        double db = 0;
        decimal dm = 0;
        Assert.Equal(fl.SizeOf(), sizeof(float));
        Assert.Equal(db.SizeOf(), sizeof(double));
        Assert.Equal(dm.SizeOf(), sizeof(decimal));

        nint ni = 0;
        nuint nu = 0;
        Assert.True(ni.SizeOf() is 4 or 8);
        Assert.True(nu.SizeOf() is 4 or 8);

        bool b = true;
        Assert.Equal(b.SizeOf(), sizeof(bool));

        //bool? nullable_b = null;
        //Assert.Equal(nullable_b.SizeOf(), sizeof(bool));

        TestStruct1 t1 = new();
        TestStruct2 t2 = new();
        TestStruct3 t3 = new();
        Assert.True(t1.SizeOf() is 1 or 4 or 8);
        Assert.True(t2.SizeOf() is 2 or 4 or 8);
        Assert.True(t3.SizeOf() is 3 or 4 or 8);

        TestEnum e = TestEnum.Aaa;
        Assert.Equal(sizeof(TestEnum), e.SizeOf());

        //string s = "abc";
        //Assert.True(s.SizeOf() is 4 or 8);

        var tc = new TestClass();
        //Assert.True(tc.SizeOf() is 4 or 8);
        //Assert.Equal(sizeof(tc.Byte), sizeof(byte));
        Assert.Equal(tc.Byte.SizeOf(), sizeof(byte));
    }
}
