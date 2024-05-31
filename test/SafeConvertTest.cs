﻿using Mina.Extension;
using System;
using Xunit;

namespace Mina.Test;

public class SafeConvertTest
{
    [Fact]
    public void ToBoolTest()
    {
        Assert.Equal(SafeConvert.ToBool(false), false);
        Assert.Equal(SafeConvert.ToBool(true), true);
        Assert.Equal(SafeConvert.ToBool(0), false);
        Assert.Equal(SafeConvert.ToBool(1), true);
        Assert.Equal(SafeConvert.ToBool(2), true);
        Assert.Equal(SafeConvert.ToBool(-1), true);
        Assert.Equal(SafeConvert.ToBool(""), false);
        Assert.Equal(SafeConvert.ToBool("False"), false);
        Assert.Equal(SafeConvert.ToBool("True"), true);
        Assert.Equal(SafeConvert.ToBool("0"), false);
        Assert.Equal(SafeConvert.ToBool("1"), false);
        Assert.Equal(SafeConvert.ToBool(null), false);
        Assert.Equal(SafeConvert.ToBool(new object()), false);
    }

    [Fact]
    public void ToSByteTest()
    {
        Assert.Equal(SafeConvert.ToSByte(false), 0);
        Assert.Equal(SafeConvert.ToSByte(true), 1);
        Assert.Equal(SafeConvert.ToSByte(0), 0);
        Assert.Equal(SafeConvert.ToSByte(1), 1);
        Assert.Equal(SafeConvert.ToSByte(127), 127);
        Assert.Equal(SafeConvert.ToSByte(128), 0);
        Assert.Equal(SafeConvert.ToSByte(-1), -1);
        Assert.Equal(SafeConvert.ToSByte(-128), -128);
        Assert.Equal(SafeConvert.ToSByte(-129), 0);
        Assert.Equal(SafeConvert.ToSByte(""), 0);
        Assert.Equal(SafeConvert.ToSByte("False"), 0);
        Assert.Equal(SafeConvert.ToSByte("True"), 0);
        Assert.Equal(SafeConvert.ToSByte("0"), 0);
        Assert.Equal(SafeConvert.ToSByte("1"), 1);
        Assert.Equal(SafeConvert.ToSByte(null), 0);
        Assert.Equal(SafeConvert.ToSByte(new object()), 0);
    }

    [Fact]
    public void ToShortTest()
    {
        Assert.Equal(SafeConvert.ToShort(false), 0);
        Assert.Equal(SafeConvert.ToShort(true), 1);
        Assert.Equal(SafeConvert.ToShort(0), 0);
        Assert.Equal(SafeConvert.ToShort(1), 1);
        Assert.Equal(SafeConvert.ToShort(32767), 32767);
        Assert.Equal(SafeConvert.ToShort(32768), 0);
        Assert.Equal(SafeConvert.ToShort(-1), -1);
        Assert.Equal(SafeConvert.ToShort(-32768), -32768);
        Assert.Equal(SafeConvert.ToShort(-32769), 0);
        Assert.Equal(SafeConvert.ToShort(""), 0);
        Assert.Equal(SafeConvert.ToShort("False"), 0);
        Assert.Equal(SafeConvert.ToShort("True"), 0);
        Assert.Equal(SafeConvert.ToShort("0"), 0);
        Assert.Equal(SafeConvert.ToShort("1"), 1);
        Assert.Equal(SafeConvert.ToShort(null), 0);
        Assert.Equal(SafeConvert.ToShort(new object()), 0);
    }

    [Fact]
    public void ToIntTest()
    {
        Assert.Equal(SafeConvert.ToInt(false), 0);
        Assert.Equal(SafeConvert.ToInt(true), 1);
        Assert.Equal(SafeConvert.ToInt(0), 0);
        Assert.Equal(SafeConvert.ToInt(1), 1);
        Assert.Equal(SafeConvert.ToInt(2147483647), 2147483647);
        Assert.Equal(SafeConvert.ToInt(2147483648), 0);
        Assert.Equal(SafeConvert.ToInt(-1), -1);
        Assert.Equal(SafeConvert.ToInt(-2147483648), -2147483648);
        Assert.Equal(SafeConvert.ToInt(-2147483649), 0);
        Assert.Equal(SafeConvert.ToInt(""), 0);
        Assert.Equal(SafeConvert.ToInt("False"), 0);
        Assert.Equal(SafeConvert.ToInt("True"), 0);
        Assert.Equal(SafeConvert.ToInt("0"), 0);
        Assert.Equal(SafeConvert.ToInt("1"), 1);
        Assert.Equal(SafeConvert.ToInt(null), 0);
        Assert.Equal(SafeConvert.ToInt(new object()), 0);
    }

    [Fact]
    public void ToLongTest()
    {
        Assert.Equal(SafeConvert.ToLong(false), 0);
        Assert.Equal(SafeConvert.ToLong(true), 1);
        Assert.Equal(SafeConvert.ToLong(0), 0);
        Assert.Equal(SafeConvert.ToLong(1), 1);
        Assert.Equal(SafeConvert.ToLong(9223372036854775807), 9223372036854775807);
        Assert.Equal(SafeConvert.ToLong(9223372036854775808), 0);
        Assert.Equal(SafeConvert.ToLong(-1), -1);
        Assert.Equal(SafeConvert.ToLong(-9223372036854775808), -9223372036854775808);
        Assert.Equal(SafeConvert.ToLong(-9223372036854775809m), 0);
        Assert.Equal(SafeConvert.ToLong(""), 0);
        Assert.Equal(SafeConvert.ToLong("False"), 0);
        Assert.Equal(SafeConvert.ToLong("True"), 0);
        Assert.Equal(SafeConvert.ToLong("0"), 0);
        Assert.Equal(SafeConvert.ToLong("1"), 1);
        Assert.Equal(SafeConvert.ToLong(null), 0);
        Assert.Equal(SafeConvert.ToLong(new object()), 0);
    }

    [Fact]
    public void ToByteTest()
    {
        Assert.Equal(SafeConvert.ToByte(false), 0);
        Assert.Equal(SafeConvert.ToByte(true), 1);
        Assert.Equal(SafeConvert.ToByte(0), 0);
        Assert.Equal(SafeConvert.ToByte(1), 1);
        Assert.Equal(SafeConvert.ToByte(255), 255);
        Assert.Equal(SafeConvert.ToByte(256), 0);
        Assert.Equal(SafeConvert.ToByte(-1), 0);
        Assert.Equal(SafeConvert.ToByte(""), 0);
        Assert.Equal(SafeConvert.ToByte("False"), 0);
        Assert.Equal(SafeConvert.ToByte("True"), 0);
        Assert.Equal(SafeConvert.ToByte("0"), 0);
        Assert.Equal(SafeConvert.ToByte("1"), 1);
        Assert.Equal(SafeConvert.ToByte(null), 0);
        Assert.Equal(SafeConvert.ToByte(new object()), 0);
    }

    [Fact]
    public void ToUShortTest()
    {
        Assert.Equal(SafeConvert.ToUShort(false), 0);
        Assert.Equal(SafeConvert.ToUShort(true), 1);
        Assert.Equal(SafeConvert.ToUShort(0), 0);
        Assert.Equal(SafeConvert.ToUShort(1), 1);
        Assert.Equal(SafeConvert.ToUShort(65535), 65535);
        Assert.Equal(SafeConvert.ToUShort(65536), 0);
        Assert.Equal(SafeConvert.ToUShort(-1), 0);
        Assert.Equal(SafeConvert.ToUShort(""), 0);
        Assert.Equal(SafeConvert.ToUShort("False"), 0);
        Assert.Equal(SafeConvert.ToUShort("True"), 0);
        Assert.Equal(SafeConvert.ToUShort("0"), 0);
        Assert.Equal(SafeConvert.ToUShort("1"), 1);
        Assert.Equal(SafeConvert.ToUShort(null), 0);
        Assert.Equal(SafeConvert.ToUShort(new object()), 0);
    }

    [Fact]
    public void ToUIntTest()
    {
        Assert.Equal(SafeConvert.ToUInt(false), 0u);
        Assert.Equal(SafeConvert.ToUInt(true), 1u);
        Assert.Equal(SafeConvert.ToUInt(0), 0u);
        Assert.Equal(SafeConvert.ToUInt(1), 1u);
        Assert.Equal(SafeConvert.ToUInt(4294967295), 4294967295u);
        Assert.Equal(SafeConvert.ToUInt(4294967296), 0u);
        Assert.Equal(SafeConvert.ToUInt(-1), 0u);
        Assert.Equal(SafeConvert.ToUInt(""), 0u);
        Assert.Equal(SafeConvert.ToUInt("False"), 0u);
        Assert.Equal(SafeConvert.ToUInt("True"), 0u);
        Assert.Equal(SafeConvert.ToUInt("0"), 0u);
        Assert.Equal(SafeConvert.ToUInt("1"), 1u);
        Assert.Equal(SafeConvert.ToUInt(null), 0u);
        Assert.Equal(SafeConvert.ToUInt(new object()), 0u);
    }

    [Fact]
    public void ToULongTest()
    {
        Assert.Equal(SafeConvert.ToULong(false), 0ul);
        Assert.Equal(SafeConvert.ToULong(true), 1ul);
        Assert.Equal(SafeConvert.ToULong(0), 0ul);
        Assert.Equal(SafeConvert.ToULong(1), 1ul);
        Assert.Equal(SafeConvert.ToULong(18446744073709551615), 18446744073709551615ul);
        Assert.Equal(SafeConvert.ToULong(18446744073709551616m), 0ul);
        Assert.Equal(SafeConvert.ToULong(-1), 0ul);
        Assert.Equal(SafeConvert.ToULong(""), 0ul);
        Assert.Equal(SafeConvert.ToULong("False"), 0ul);
        Assert.Equal(SafeConvert.ToULong("True"), 0ul);
        Assert.Equal(SafeConvert.ToULong("0"), 0ul);
        Assert.Equal(SafeConvert.ToULong("1"), 1ul);
        Assert.Equal(SafeConvert.ToULong(null), 0ul);
        Assert.Equal(SafeConvert.ToULong(new object()), 0ul);
    }

    [Fact]
    public void ToFloatTest()
    {
        Assert.Equal(SafeConvert.ToFloat(false), 0f);
        Assert.Equal(SafeConvert.ToFloat(true), 1f);
        Assert.Equal(SafeConvert.ToFloat(0), 0f);
        Assert.Equal(SafeConvert.ToFloat(0.1), 0.1f);
        Assert.Equal(SafeConvert.ToFloat(-0.1), -0.1f);
        Assert.Equal(SafeConvert.ToFloat(""), 0f);
        Assert.Equal(SafeConvert.ToFloat("False"), 0f);
        Assert.Equal(SafeConvert.ToFloat("True"), 0f);
        Assert.Equal(SafeConvert.ToFloat("0"), 0f);
        Assert.Equal(SafeConvert.ToFloat("1"), 1f);
        Assert.Equal(SafeConvert.ToFloat(null), 0f);
        Assert.Equal(SafeConvert.ToFloat(new object()), 0f);
    }

    [Fact]
    public void ToDoubleTest()
    {
        Assert.Equal(SafeConvert.ToDouble(false), 0d);
        Assert.Equal(SafeConvert.ToDouble(true), 1d);
        Assert.Equal(SafeConvert.ToDouble(0), 0d);
        Assert.Equal(SafeConvert.ToDouble(0.1), 0.1d);
        Assert.Equal(SafeConvert.ToDouble(-0.1), -0.1d);
        Assert.Equal(SafeConvert.ToDouble(""), 0d);
        Assert.Equal(SafeConvert.ToDouble("False"), 0d);
        Assert.Equal(SafeConvert.ToDouble("True"), 0d);
        Assert.Equal(SafeConvert.ToDouble("0"), 0d);
        Assert.Equal(SafeConvert.ToDouble("1"), 1d);
        Assert.Equal(SafeConvert.ToDouble(null), 0d);
        Assert.Equal(SafeConvert.ToDouble(new object()), 0d);
    }

    [Fact]
    public void ToDecimalTest()
    {
        Assert.Equal(SafeConvert.ToDecimal(false), 0m);
        Assert.Equal(SafeConvert.ToDecimal(true), 1m);
        Assert.Equal(SafeConvert.ToDecimal(0), 0m);
        Assert.Equal(SafeConvert.ToDecimal(0.1), 0.1m);
        Assert.Equal(SafeConvert.ToDecimal(-0.1), -0.1m);
        Assert.Equal(SafeConvert.ToDecimal(""), 0m);
        Assert.Equal(SafeConvert.ToDecimal("False"), 0m);
        Assert.Equal(SafeConvert.ToDecimal("True"), 0m);
        Assert.Equal(SafeConvert.ToDecimal("0"), 0m);
        Assert.Equal(SafeConvert.ToDecimal("1"), 1m);
        Assert.Equal(SafeConvert.ToDecimal(null), 0m);
        Assert.Equal(SafeConvert.ToDecimal(new object()), 0m);
    }

    [Fact]
    public void ToDateTimeTest()
    {
        DateTime dt = default;

        Assert.Equal(SafeConvert.ToDateTime(false), dt);
        Assert.Equal(SafeConvert.ToDateTime(true), dt);
        Assert.Equal(SafeConvert.ToDateTime(0), dt);
        Assert.Equal(SafeConvert.ToDateTime(0.1), dt);
        Assert.Equal(SafeConvert.ToDateTime(-0.1), dt);
        Assert.Equal(SafeConvert.ToDateTime(new DateTime(2000, 12, 31)), new DateTime(2000, 12, 31));
        Assert.Equal(SafeConvert.ToDateTime("2000/12/31"), new DateTime(2000, 12, 31));
        Assert.Equal(SafeConvert.ToDateTime("2000/12/31 23:59"), new DateTime(2000, 12, 31, 23, 59, 0));
        Assert.Equal(SafeConvert.ToDateTime("2000/12/31 23:59:45"), new DateTime(2000, 12, 31, 23, 59, 45));
        Assert.Equal(SafeConvert.ToDateTime("2000/12/31 23:59:45.123"), new DateTime(2000, 12, 31, 23, 59, 45, 123));
        Assert.Equal(SafeConvert.ToDateTime("2000/12/31 23:59:45.123456"), new DateTime(2000, 12, 31, 23, 59, 45, 123, 456));
        Assert.Equal(SafeConvert.ToDateTime("2000/12/32"), dt);
        Assert.Equal(SafeConvert.ToDateTime("2000/12/31 23:60"), dt);
        Assert.Equal(SafeConvert.ToDateTime("2000/12/31 23:59:60"), dt);
        Assert.Equal(SafeConvert.ToDateTime(""), dt);
        Assert.Equal(SafeConvert.ToDateTime("False"), dt);
        Assert.Equal(SafeConvert.ToDateTime("True"), dt);
        Assert.Equal(SafeConvert.ToDateTime("0"), dt);
        Assert.Equal(SafeConvert.ToDateTime("1"), dt);
        Assert.Equal(SafeConvert.ToDateTime(null), dt);
        Assert.Equal(SafeConvert.ToDateTime(new object()), dt);
    }

    public class ToStringBinder(Func<string?> f)
    {
        public override string ToString() => f()!;
    }

    [Fact]
    public void ToStringTest()
    {
        Assert.Equal(SafeConvert.ToString(false), "False");
        Assert.Equal(SafeConvert.ToString(true), "True");
        Assert.Equal(SafeConvert.ToString(0), "0");
        Assert.Equal(SafeConvert.ToString(0.1), "0.1");
        Assert.Equal(SafeConvert.ToString(-0.1), "-0.1");
        Assert.Equal(SafeConvert.ToString(""), "");
        Assert.Equal(SafeConvert.ToString("False"), "False");
        Assert.Equal(SafeConvert.ToString("True"), "True");
        Assert.Equal(SafeConvert.ToString("0"), "0");
        Assert.Equal(SafeConvert.ToString("1"), "1");
        Assert.Equal(SafeConvert.ToString(null), "");
        Assert.Equal(SafeConvert.ToString(new object()), "System.Object");
        Assert.Equal(SafeConvert.ToString(new ToStringBinder(() => "test")), "test");
        Assert.Equal(SafeConvert.ToString(new ToStringBinder(() => null)), "");
        Assert.Equal(SafeConvert.ToString(new ToStringBinder(() => throw new())), "");
    }

    [Fact]
    public void ToBoolOrNullTest()
    {
        Assert.Equal(SafeConvert.ToBoolOrNull(false), false);
        Assert.Equal(SafeConvert.ToBoolOrNull(true), true);
        Assert.Equal(SafeConvert.ToBoolOrNull(0), false);
        Assert.Equal(SafeConvert.ToBoolOrNull(1), true);
        Assert.Equal(SafeConvert.ToBoolOrNull(2), true);
        Assert.Equal(SafeConvert.ToBoolOrNull(-1), true);
        Assert.Equal(SafeConvert.ToBoolOrNull(""), null);
        Assert.Equal(SafeConvert.ToBoolOrNull("False"), false);
        Assert.Equal(SafeConvert.ToBoolOrNull("True"), true);
        Assert.Equal(SafeConvert.ToBoolOrNull("0"), null);
        Assert.Equal(SafeConvert.ToBoolOrNull("1"), null);
        Assert.Equal(SafeConvert.ToBoolOrNull(null), null);
        Assert.Equal(SafeConvert.ToBoolOrNull(new object()), null);
    }

    [Fact]
    public void ToSByteOrNullTest()
    {
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(false), 0);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(true), 1);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(0), 0);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(1), 1);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(127), 127);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(128), null);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(-1), -1);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(-128), -128);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(-129), null);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(""), null);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull("False"), null);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull("True"), null);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull("0"), 0);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull("1"), 1);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(null), null);
        Assert.Equal<sbyte?>(SafeConvert.ToSByteOrNull(new object()), null);
    }

    [Fact]
    public void ToShortOrNullTest()
    {
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(false), 0);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(true), 1);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(0), 0);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(1), 1);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(32767), 32767);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(32768), null);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(-1), -1);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(-32768), -32768);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(-32769), null);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(""), null);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull("False"), null);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull("True"), null);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull("0"), 0);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull("1"), 1);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(null), null);
        Assert.Equal<short?>(SafeConvert.ToShortOrNull(new object()), null);
    }

    [Fact]
    public void ToIntOrNullTest()
    {
        Assert.Equal(SafeConvert.ToIntOrNull(false), 0);
        Assert.Equal(SafeConvert.ToIntOrNull(true), 1);
        Assert.Equal(SafeConvert.ToIntOrNull(0), 0);
        Assert.Equal(SafeConvert.ToIntOrNull(1), 1);
        Assert.Equal(SafeConvert.ToIntOrNull(2147483647), 2147483647);
        Assert.Equal(SafeConvert.ToIntOrNull(2147483648), null);
        Assert.Equal(SafeConvert.ToIntOrNull(-1), -1);
        Assert.Equal(SafeConvert.ToIntOrNull(-2147483648), -2147483648);
        Assert.Equal(SafeConvert.ToIntOrNull(-2147483649), null);
        Assert.Equal(SafeConvert.ToIntOrNull(""), null);
        Assert.Equal(SafeConvert.ToIntOrNull("False"), null);
        Assert.Equal(SafeConvert.ToIntOrNull("True"), null);
        Assert.Equal(SafeConvert.ToIntOrNull("0"), 0);
        Assert.Equal(SafeConvert.ToIntOrNull("1"), 1);
        Assert.Equal(SafeConvert.ToIntOrNull(null), null);
        Assert.Equal(SafeConvert.ToIntOrNull(new object()), null);
    }

    [Fact]
    public void ToLongOrNullTest()
    {
        Assert.Equal(SafeConvert.ToLongOrNull(false), 0);
        Assert.Equal(SafeConvert.ToLongOrNull(true), 1);
        Assert.Equal(SafeConvert.ToLongOrNull(0), 0);
        Assert.Equal(SafeConvert.ToLongOrNull(1), 1);
        Assert.Equal(SafeConvert.ToLongOrNull(9223372036854775807), 9223372036854775807);
        Assert.Equal(SafeConvert.ToLongOrNull(9223372036854775808), null);
        Assert.Equal(SafeConvert.ToLongOrNull(-1), -1);
        Assert.Equal(SafeConvert.ToLongOrNull(-9223372036854775808), -9223372036854775808);
        Assert.Equal(SafeConvert.ToLongOrNull(-9223372036854775809m), null);
        Assert.Equal(SafeConvert.ToLongOrNull(""), null);
        Assert.Equal(SafeConvert.ToLongOrNull("False"), null);
        Assert.Equal(SafeConvert.ToLongOrNull("True"), null);
        Assert.Equal(SafeConvert.ToLongOrNull("0"), 0);
        Assert.Equal(SafeConvert.ToLongOrNull("1"), 1);
        Assert.Equal(SafeConvert.ToLongOrNull(null), null);
        Assert.Equal(SafeConvert.ToLongOrNull(new object()), null);
    }

    [Fact]
    public void ToByteOrNullTest()
    {
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(false), 0);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(true), 1);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(0), 0);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(1), 1);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(255), 255);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(256), null);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(-1), null);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(""), null);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull("False"), null);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull("True"), null);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull("0"), 0);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull("1"), 1);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(null), null);
        Assert.Equal<byte?>(SafeConvert.ToByteOrNull(new object()), null);
    }

    [Fact]
    public void ToUShortOrNullTest()
    {
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(false), 0);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(true), 1);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(0), 0);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(1), 1);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(65535), 65535);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(65536), null);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(-1), null);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(""), null);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull("False"), null);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull("True"), null);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull("0"), 0);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull("1"), 1);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(null), null);
        Assert.Equal<ushort?>(SafeConvert.ToUShortOrNull(new object()), null);
    }

    [Fact]
    public void ToUIntOrNullTest()
    {
        Assert.Equal(SafeConvert.ToUIntOrNull(false), 0u);
        Assert.Equal(SafeConvert.ToUIntOrNull(true), 1u);
        Assert.Equal(SafeConvert.ToUIntOrNull(0), 0u);
        Assert.Equal(SafeConvert.ToUIntOrNull(1), 1u);
        Assert.Equal(SafeConvert.ToUIntOrNull(4294967295), 4294967295u);
        Assert.Equal(SafeConvert.ToUIntOrNull(4294967296), null);
        Assert.Equal(SafeConvert.ToUIntOrNull(-1), null);
        Assert.Equal(SafeConvert.ToUIntOrNull(""), null);
        Assert.Equal(SafeConvert.ToUIntOrNull("False"), null);
        Assert.Equal(SafeConvert.ToUIntOrNull("True"), null);
        Assert.Equal(SafeConvert.ToUIntOrNull("0"), 0u);
        Assert.Equal(SafeConvert.ToUIntOrNull("1"), 1u);
        Assert.Equal(SafeConvert.ToUIntOrNull(null), null);
        Assert.Equal(SafeConvert.ToUIntOrNull(new object()), null);
    }

    [Fact]
    public void ToULongOrNullTest()
    {
        Assert.Equal(SafeConvert.ToULongOrNull(false), 0ul);
        Assert.Equal(SafeConvert.ToULongOrNull(true), 1ul);
        Assert.Equal(SafeConvert.ToULongOrNull(0), 0ul);
        Assert.Equal(SafeConvert.ToULongOrNull(1), 1ul);
        Assert.Equal(SafeConvert.ToULongOrNull(18446744073709551615), 18446744073709551615ul);
        Assert.Equal(SafeConvert.ToULongOrNull(18446744073709551616m), null);
        Assert.Equal(SafeConvert.ToULongOrNull(-1), null);
        Assert.Equal(SafeConvert.ToULongOrNull(""), null);
        Assert.Equal(SafeConvert.ToULongOrNull("False"), null);
        Assert.Equal(SafeConvert.ToULongOrNull("True"), null);
        Assert.Equal(SafeConvert.ToULongOrNull("0"), 0ul);
        Assert.Equal(SafeConvert.ToULongOrNull("1"), 1ul);
        Assert.Equal(SafeConvert.ToULongOrNull(null), null);
        Assert.Equal(SafeConvert.ToULongOrNull(new object()), null);
    }

    [Fact]
    public void ToFloatOrNullTest()
    {
        Assert.Equal(SafeConvert.ToFloatOrNull(false), 0f);
        Assert.Equal(SafeConvert.ToFloatOrNull(true), 1f);
        Assert.Equal(SafeConvert.ToFloatOrNull(0), 0f);
        Assert.Equal(SafeConvert.ToFloatOrNull(0.1), 0.1f);
        Assert.Equal(SafeConvert.ToFloatOrNull(-0.1), -0.1f);
        Assert.Equal(SafeConvert.ToFloatOrNull(""), null);
        Assert.Equal(SafeConvert.ToFloatOrNull("False"), null);
        Assert.Equal(SafeConvert.ToFloatOrNull("True"), null);
        Assert.Equal(SafeConvert.ToFloatOrNull("0"), 0f);
        Assert.Equal(SafeConvert.ToFloatOrNull("1"), 1f);
        Assert.Equal(SafeConvert.ToFloatOrNull(null), null);
        Assert.Equal(SafeConvert.ToFloatOrNull(new object()), null);
    }

    [Fact]
    public void ToDoubleOrNullTest()
    {
        Assert.Equal(SafeConvert.ToDoubleOrNull(false), 0d);
        Assert.Equal(SafeConvert.ToDoubleOrNull(true), 1d);
        Assert.Equal(SafeConvert.ToDoubleOrNull(0), 0d);
        Assert.Equal(SafeConvert.ToDoubleOrNull(0.1), 0.1d);
        Assert.Equal(SafeConvert.ToDoubleOrNull(-0.1), -0.1d);
        Assert.Equal(SafeConvert.ToDoubleOrNull(""), null);
        Assert.Equal(SafeConvert.ToDoubleOrNull("False"), null);
        Assert.Equal(SafeConvert.ToDoubleOrNull("True"), null);
        Assert.Equal(SafeConvert.ToDoubleOrNull("0"), 0d);
        Assert.Equal(SafeConvert.ToDoubleOrNull("1"), 1d);
        Assert.Equal(SafeConvert.ToDoubleOrNull(null), null);
        Assert.Equal(SafeConvert.ToDoubleOrNull(new object()), null);
    }

    [Fact]
    public void ToDecimalOrNullTest()
    {
        Assert.Equal(SafeConvert.ToDecimalOrNull(false), 0m);
        Assert.Equal(SafeConvert.ToDecimalOrNull(true), 1m);
        Assert.Equal(SafeConvert.ToDecimalOrNull(0), 0m);
        Assert.Equal(SafeConvert.ToDecimalOrNull(0.1), 0.1m);
        Assert.Equal(SafeConvert.ToDecimalOrNull(-0.1), -0.1m);
        Assert.Equal(SafeConvert.ToDecimalOrNull(""), null);
        Assert.Equal(SafeConvert.ToDecimalOrNull("False"), null);
        Assert.Equal(SafeConvert.ToDecimalOrNull("True"), null);
        Assert.Equal(SafeConvert.ToDecimalOrNull("0"), 0m);
        Assert.Equal(SafeConvert.ToDecimalOrNull("1"), 1m);
        Assert.Equal(SafeConvert.ToDecimalOrNull(null), null);
        Assert.Equal(SafeConvert.ToDecimalOrNull(new object()), null);
    }

    [Fact]
    public void ToDateTimeOrNullTest()
    {
        DateTime? dt = default;

        Assert.Equal(SafeConvert.ToDateTimeOrNull(false), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(true), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(0), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(0.1), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(-0.1), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(new DateTime(2000, 12, 31)), new DateTime(2000, 12, 31));
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/31"), new DateTime(2000, 12, 31));
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/31 23:59"), new DateTime(2000, 12, 31, 23, 59, 0));
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/31 23:59:45"), new DateTime(2000, 12, 31, 23, 59, 45));
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/31 23:59:45.123"), new DateTime(2000, 12, 31, 23, 59, 45, 123));
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/31 23:59:45.123456"), new DateTime(2000, 12, 31, 23, 59, 45, 123, 456));
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/32"), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/31 23:60"), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull("2000/12/31 23:59:60"), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(""), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull("False"), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull("True"), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull("0"), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull("1"), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(null), dt);
        Assert.Equal(SafeConvert.ToDateTimeOrNull(new object()), dt);
    }

    [Fact]
    public void ToStringOrNullTest()
    {
        Assert.Equal(SafeConvert.ToStringOrNull(false), "False");
        Assert.Equal(SafeConvert.ToStringOrNull(true), "True");
        Assert.Equal(SafeConvert.ToStringOrNull(0), "0");
        Assert.Equal(SafeConvert.ToStringOrNull(0.1), "0.1");
        Assert.Equal(SafeConvert.ToStringOrNull(-0.1), "-0.1");
        Assert.Equal(SafeConvert.ToStringOrNull(""), "");
        Assert.Equal(SafeConvert.ToStringOrNull("False"), "False");
        Assert.Equal(SafeConvert.ToStringOrNull("True"), "True");
        Assert.Equal(SafeConvert.ToStringOrNull("0"), "0");
        Assert.Equal(SafeConvert.ToStringOrNull("1"), "1");
        Assert.Equal(SafeConvert.ToStringOrNull(null), null);
        Assert.Equal(SafeConvert.ToStringOrNull(new object()), "System.Object");
        Assert.Equal(SafeConvert.ToStringOrNull(new ToStringBinder(() => "test")), "test");
        Assert.Equal(SafeConvert.ToStringOrNull(new ToStringBinder(() => null)), null);
        Assert.Equal(SafeConvert.ToStringOrNull(new ToStringBinder(() => throw new())), null);
    }
}
