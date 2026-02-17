using Mina.Extension;
using System;
using System.Text;
using Xunit;

namespace Mina.Test;

public class EncodingsTest
{
    [Fact]
    public void GetEncodingTest()
    {
        Assert.Throws<ArgumentException>(() => Encoding.GetEncoding(""));
        Assert.Throws<ArgumentException>(() => Encoding.GetEncoding("xxx_not_found_xxx"));
        Assert.Throws<NotSupportedException>(() => Encoding.GetEncoding(65535));
        Assert.Throws<ArgumentOutOfRangeException>(() => Encoding.GetEncoding(-1));
        Assert.Equal(Encoding.GetEncoding(1200), Encoding.Unicode);
    }

    [Fact]
    public void GetEncodingOrNullTest()
    {
        Assert.Equal(Encodings.GetEncodingOrNull(""), null);
        Assert.Equal(Encodings.GetEncodingOrNull("xxx_not_found_xxx"), null);
        Assert.Equal(Encodings.GetEncodingOrNull(65535), null);
        Assert.Equal(Encodings.GetEncodingOrNull(-1), null);
        Assert.Equal(Encodings.GetEncodingOrNull(1200), Encoding.Unicode);
    }

    [Fact]
    public void GetEncodingOrUtf8Test()
    {
        var utf8 = Encoding.UTF8;

        Assert.Equal(Encodings.GetEncodingOrUtf8(""), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8("xxx_not_found_xxx"), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8(65535), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8(-1), utf8);
        Assert.Equal(Encodings.GetEncodingOrUtf8(1200), Encoding.Unicode);
    }

    [Fact]
    public void GetByteCountOrNullTest()
    {
        var encoding = Encoding.ASCII;
        Assert.Equal(encoding.GetByteCountOrNull(""), 0);
        Assert.Equal(encoding.GetByteCountOrNull("abc"), 3);
        Assert.Equal(encoding.GetByteCountOrNull("abcあいう"), null);
        Assert.Equal(encoding.GetByteCountOrNull("🍣"), null);
        Assert.Equal(encoding.GetByteCountOrNull("abcあいう🍣"), null);

        Assert.Equal(encoding.GetByteCount(""), 0);
        Assert.Equal(encoding.GetByteCount("abc"), 3);
        Assert.Equal(encoding.GetByteCount("abcあいう"), 6);
        Assert.Equal(encoding.GetByteCount("🍣"), 2);
        Assert.Equal(encoding.GetByteCount("abcあいう🍣"), 8);
    }

    [Fact]
    public void GetBytesOrNullTest()
    {
        var encoding = Encoding.ASCII;
        Assert.Equal(encoding.GetBytesOrNull(""), new byte[] { });
        Assert.Equal(encoding.GetBytesOrNull("abc"), new byte[] { 97, 98, 99 });
        Assert.Equal(encoding.GetBytesOrNull("abcあいう"), null);
        Assert.Equal(encoding.GetBytesOrNull("🍣"), null);
        Assert.Equal(encoding.GetBytesOrNull("abcあいう🍣"), null);

        Assert.Equal(encoding.GetBytes(""), new byte[] { });
        Assert.Equal(encoding.GetBytes("abc"), new byte[] { 97, 98, 99 });
        Assert.Equal(encoding.GetBytes("abcあいう"), new byte[] { 97, 98, 99, 63, 63, 63 });
        Assert.Equal(encoding.GetBytes("🍣"), new byte[] { 63, 63 });
        Assert.Equal(encoding.GetBytes("abcあいう🍣"), new byte[] { 97, 98, 99, 63, 63, 63, 63, 63 });
    }

    [Fact]
    public void GetCharCountOrNullTest()
    {
        var encoding = Encoding.ASCII;
        Assert.Equal(encoding.GetCharCountOrNull([]), 0);
        Assert.Equal(encoding.GetCharCountOrNull([97, 98, 99]), 3);
        Assert.Equal(encoding.GetCharCountOrNull([97, 98, 99, 255]), null);

        Assert.Equal(encoding.GetCharCount([]), 0);
        Assert.Equal(encoding.GetCharCount([97, 98, 99]), 3);
        Assert.Equal(encoding.GetCharCount([97, 98, 99, 255]), 4);
    }

    [Fact]
    public void GetStringOrNullTest()
    {
        var encoding = Encoding.ASCII;
        Assert.Equal(encoding.GetStringOrNull([]), "");
        Assert.Equal(encoding.GetStringOrNull([97, 98, 99]), "abc");
        Assert.Equal(encoding.GetStringOrNull([07, 98, 99, 255]), null);

        Assert.Equal(encoding.GetString([]), "");
        Assert.Equal(encoding.GetString([97, 98, 99]), "abc");
        Assert.Equal(encoding.GetString([97, 98, 99, 255]), "abc?");
    }
}
