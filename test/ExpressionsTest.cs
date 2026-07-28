using Mina.Reflection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Xunit;

namespace Mina.Test;

public class ExpressionsTest
{
    [Fact]
    public void AddTest()
    {
        var add_int = Expressions.Add<int>();
        Assert.Equal(add_int(1, 2), 3);

        var add_double = Expressions.Add<double>();
        var d = add_double(1.1, 2.2);
        Assert.True(d >= 3.30);
        Assert.True(d <= 3.31);
    }

    [Fact]
    public void SubTest()
    {
        var sub_int = Expressions.Subtract<int>();
        Assert.Equal(sub_int(1, 2), -1);
    }

    [Fact]
    public void MulTest()
    {
        var mul_int = Expressions.Multiply<int>();
        Assert.Equal(mul_int(3, 4), 12);
    }

    [Fact]
    public void DivTest()
    {
        var div_int = Expressions.Divide<int>();
        Assert.Equal(div_int(7, 3), 2);
    }

    [Fact]
    public void ModTest()
    {
        var mod_int = Expressions.Modulo<int>();
        Assert.Equal(mod_int(8, 3), 2);
    }

    [Fact]
    public void LShiftTest()
    {
        var lshift_int = Expressions.LeftShift<int>();
        Assert.Equal(lshift_int(5, 3), 40);
    }

    [Fact]
    public void RShiftTest()
    {
        var rshift_int = Expressions.RightShift<int>();
        Assert.Equal(rshift_int(40, 3), 5);
    }

    public class ParsableClass : IParsable<ParsableClass>
    {
        public required int Value { get; init; }

        public static ParsableClass Parse(string s, IFormatProvider? provider) => TryParse(s, provider, out var result) ? result : throw new();

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ParsableClass result)
        {
            var b = int.TryParse(s, out var r);
            result = b ? new ParsableClass() { Value = r } : default;
            return b;
        }
    }

    public class SpanParsableClass : ISpanParsable<SpanParsableClass>
    {
        public required int Value { get; init; }

        public static SpanParsableClass Parse(string s, IFormatProvider? provider) => TryParse(s, provider, out var result) ? result : throw new();

        public static SpanParsableClass Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => TryParse(s, provider, out var result) ? result : throw new();

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SpanParsableClass result) => TryParse(s.AsSpan(), provider, out result);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out SpanParsableClass result)
        {
            var b = int.TryParse(s, out var r);
            result = b ? new SpanParsableClass() { Value = r } : default;
            return b;
        }
    }

    [Fact]
    public void TryConvertTest()
    {
        var result1 = Expressions.TryConvert(typeof(DateTime), "a", out var _);
        Assert.Equal(result1, false);

        var result2 = Expressions.TryConvert(typeof(DateTime), "2000/01/02", out var v2);
        Assert.Equal(result2, true);
        Assert.Equal(v2, new DateTime(2000, 1, 2));

        var result3 = Expressions.TryConvert(typeof(Color), "Red", out var _);
        Assert.Equal(result3, false);

        var result4 = Expressions.TryConvert(typeof(ParsableClass), "123", out var v4);
        Assert.Equal(result4, true);
        var o4 = Assert.IsType<ParsableClass>(v4);
        Assert.Equal(o4.Value, 123);

        var result5 = Expressions.TryConvert(typeof(SpanParsableClass), "234", out var v5);
        Assert.Equal(result5, true);
        var o5 = Assert.IsType<SpanParsableClass>(v5);
        Assert.Equal(o5.Value, 234);
    }
}
