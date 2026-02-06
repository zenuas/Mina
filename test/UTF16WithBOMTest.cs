using Mina.Text;
using System;
using Xunit;

namespace Mina.Test;

public class UTF16WithBOMTest
{
    [Fact]
    public void GetBytesBETest()
    {
        var enc = UTF16WithBOM.UTF16_BEWithBOM;

        Assert.Equal(enc.GetBytes(""), [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes("abc"), [0xFE, 0xFF, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63]);

        Assert.Equal(enc.GetBytes("", 0, 0), [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes("abc", 0, 0), [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes("abc", 0, 3), [0xFE, 0xFF, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63]);

        char[] empty = [];
        char[] abc = ['a', 'b', 'c'];
        Assert.Equal(enc.GetBytes(empty), [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes(abc), [0xFE, 0xFF, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63]);

        Assert.Equal(enc.GetBytes(empty, 0, 0), [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes(abc, 0, 0), [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes(abc, 0, 3), [0xFE, 0xFF, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63]);

        Span<byte> buffer = new byte[100];
        ReadOnlySpan<char> empty2 = new(empty);
        ReadOnlySpan<char> abc2 = new(abc);
        Assert.Equal(enc.GetBytes(empty2, buffer), 2);
        Assert.Equal(buffer[0..2].ToArray(), [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes(abc2, buffer), 8);
        Assert.Equal(buffer[0..8].ToArray(), [0xFE, 0xFF, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63]);

        var bytes = new byte[100];
        Assert.Equal(enc.GetBytes(empty, 0, 0, bytes, 0), 2);
        Assert.Equal(bytes[0..2], [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes(abc, 0, 0, bytes, 0), 2);
        Assert.Equal(bytes[0..2], [0xFE, 0xFF]);
        Assert.Equal(enc.GetBytes(abc, 0, 3, bytes, 0), 8);
        Assert.Equal(bytes[0..8], [0xFE, 0xFF, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63]);

        unsafe
        {
            var bytes2 = new byte[100];
            fixed (byte* bytes3 = bytes2)
            fixed (char* empty3 = empty)
            {
                Assert.Equal(enc.GetBytes(empty3, 0, bytes3, 100), 2);
                Assert.Equal(bytes2[0..2], [0xFE, 0xFF]);
            }
            fixed (byte* bytes3 = bytes2)
            fixed (char* abc3 = abc2)
            {
                Assert.Equal(enc.GetBytes(abc3, 0, bytes3, 100), 2);
                Assert.Equal(bytes2[0..2], [0xFE, 0xFF]);
                Assert.Equal(enc.GetBytes(abc3, 3, bytes3, 100), 8);
                Assert.Equal(bytes2[0..8], [0xFE, 0xFF, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63]);
            }
        }
    }

    [Fact]
    public void GetBytesLETest()
    {
        var enc = UTF16WithBOM.UTF16_LEWithBOM;

        Assert.Equal(enc.GetBytes(""), [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes("abc"), [0xFF, 0xFE, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00]);

        Assert.Equal(enc.GetBytes("", 0, 0), [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes("abc", 0, 0), [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes("abc", 0, 3), [0xFF, 0xFE, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00]);

        char[] empty = [];
        char[] abc = ['a', 'b', 'c'];
        Assert.Equal(enc.GetBytes(empty), [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes(abc), [0xFF, 0xFE, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00]);

        Assert.Equal(enc.GetBytes(empty, 0, 0), [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes(abc, 0, 0), [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes(abc, 0, 3), [0xFF, 0xFE, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00]);

        Span<byte> buffer = new byte[100];
        ReadOnlySpan<char> empty2 = new(empty);
        ReadOnlySpan<char> abc2 = new(abc);
        Assert.Equal(enc.GetBytes(empty2, buffer), 2);
        Assert.Equal(buffer[0..2].ToArray(), [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes(abc2, buffer), 8);
        Assert.Equal(buffer[0..8].ToArray(), [0xFF, 0xFE, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00]);

        var bytes = new byte[100];
        Assert.Equal(enc.GetBytes(empty, 0, 0, bytes, 0), 2);
        Assert.Equal(bytes[0..2], [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes(abc, 0, 0, bytes, 0), 2);
        Assert.Equal(bytes[0..2], [0xFF, 0xFE]);
        Assert.Equal(enc.GetBytes(abc, 0, 3, bytes, 0), 8);
        Assert.Equal(bytes[0..8], [0xFF, 0xFE, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00]);

        unsafe
        {
            var bytes2 = new byte[100];
            fixed (byte* bytes3 = bytes2)
            fixed (char* empty3 = empty)
            {
                Assert.Equal(enc.GetBytes(empty3, 0, bytes3, 100), 2);
                Assert.Equal(bytes2[0..2], [0xFF, 0xFE]);
            }
            fixed (byte* bytes3 = bytes2)
            fixed (char* abc3 = abc2)
            {
                Assert.Equal(enc.GetBytes(abc3, 0, bytes3, 100), 2);
                Assert.Equal(bytes2[0..2], [0xFF, 0xFE]);
                Assert.Equal(enc.GetBytes(abc3, 3, bytes3, 100), 8);
                Assert.Equal(bytes2[0..8], [0xFF, 0xFE, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00]);
            }
        }
    }

    [Fact]
    public void GetByteCountBETest()
    {
        var enc = UTF16WithBOM.UTF16_BEWithBOM;

        Assert.Equal(enc.GetByteCount(""), 2);
        Assert.Equal(enc.GetByteCount("abc"), 8);

        Assert.Equal(enc.GetByteCount("", 0, 0), 2);
        Assert.Equal(enc.GetByteCount("abc", 0, 0), 2);
        Assert.Equal(enc.GetByteCount("abc", 0, 3), 8);

        ReadOnlySpan<char> empty = [];
        ReadOnlySpan<char> abc = ['a', 'b', 'c'];
        Assert.Equal(enc.GetByteCount(empty), 2);
        Assert.Equal(enc.GetByteCount(abc), 8);

        char[] empty2 = [];
        char[] abc2 = ['a', 'b', 'c'];
        Assert.Equal(enc.GetByteCount(empty2), 2);
        Assert.Equal(enc.GetByteCount(abc2), 8);

        Assert.Equal(enc.GetByteCount(empty2, 0, 0), 2);
        Assert.Equal(enc.GetByteCount(abc2, 0, 0), 2);
        Assert.Equal(enc.GetByteCount(abc2, 0, 3), 8);

        unsafe
        {
            fixed (char* empty3 = empty2)
            {
                Assert.Equal(enc.GetByteCount(empty3, 0), 2);
            }
            fixed (char* abc3 = abc2)
            {
                Assert.Equal(enc.GetByteCount(abc3, 0), 2);
                Assert.Equal(enc.GetByteCount(abc3, 3), 8);
            }
        }
    }

    [Fact]
    public void GetByteCountLETest()
    {
        var enc = UTF16WithBOM.UTF16_LEWithBOM;

        Assert.Equal(enc.GetByteCount(""), 2);
        Assert.Equal(enc.GetByteCount("abc"), 8);

        Assert.Equal(enc.GetByteCount("", 0, 0), 2);
        Assert.Equal(enc.GetByteCount("abc", 0, 0), 2);
        Assert.Equal(enc.GetByteCount("abc", 0, 3), 8);

        ReadOnlySpan<char> empty = [];
        ReadOnlySpan<char> abc = ['a', 'b', 'c'];
        Assert.Equal(enc.GetByteCount(empty), 2);
        Assert.Equal(enc.GetByteCount(abc), 8);

        char[] empty2 = [];
        char[] abc2 = ['a', 'b', 'c'];
        Assert.Equal(enc.GetByteCount(empty2), 2);
        Assert.Equal(enc.GetByteCount(abc2), 8);

        Assert.Equal(enc.GetByteCount(empty2, 0, 0), 2);
        Assert.Equal(enc.GetByteCount(abc2, 0, 0), 2);
        Assert.Equal(enc.GetByteCount(abc2, 0, 3), 8);

        unsafe
        {
            fixed (char* empty3 = empty2)
            {
                Assert.Equal(enc.GetByteCount(empty3, 0), 2);
            }
            fixed (char* abc3 = abc2)
            {
                Assert.Equal(enc.GetByteCount(abc3, 0), 2);
                Assert.Equal(enc.GetByteCount(abc3, 3), 8);
            }
        }
    }
}
