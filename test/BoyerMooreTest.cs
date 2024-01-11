using Mina.Extensions;
using System;
using Xunit;

namespace Mina.Test;

public class BoyerMooreTest
{
    public static int Match(string value, string pattern, int startIndex = 0)
    {
        var normal = value.IndexOf(pattern, startIndex, StringComparison.Ordinal);
        var bm = value.IndexOf(new BoyerMooreTable(pattern), startIndex);
        if (normal != bm) throw new();
        return bm;
    }

    [Fact]
    public void IndexOfDifferenceTest()
    {
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => "".IndexOf("", 1));
        Assert.Equal("".IndexOf(new BoyerMooreTable(""), 1), -1);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => "".IndexOf("", -1));
        Assert.Equal("".IndexOf(new BoyerMooreTable(""), -1), -1);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => "a".IndexOf("a", -1));
        Assert.Equal("a".IndexOf(new BoyerMooreTable("a"), -1), -1);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => "a".IndexOf("a", 2));
        Assert.Equal("a".IndexOf(new BoyerMooreTable("a"), 2), -1);

        Assert.Equal(3, "abcあいうアイウ亜伊宇ｱｲｳ".IndexOf("アイウ", StringComparison.CurrentCulture));
        Assert.Equal(6, "abcあいうアイウ亜伊宇ｱｲｳ".IndexOf("アイウ", StringComparison.Ordinal));
        Assert.Equal(6, "abcあいうアイウ亜伊宇ｱｲｳ".IndexOf(new BoyerMooreTable("アイウ")));
    }

    [Fact]
    public void IndexOfTest()
    {
        Assert.Equal(0, Match("", ""));
        Assert.Equal(0, Match("a", ""));
        Assert.Equal(1, Match("a", "", 1));
        Assert.Equal(3, Match("abcabcx", "abcx"));
        Assert.Equal(0, Match("abcxabc", "abcx"));
        Assert.Equal(6, Match("abcabcabcxabc", "abcx"));

        Assert.Equal(-1, Match("abcabcy", "abcx"));
        Assert.Equal(-1, Match("abcyabc", "abcx"));
        Assert.Equal(-1, Match("abcabcabcyabc", "abcx"));

        Assert.Equal(0, Match("abcあいうアイウ亜伊宇ｱｲｳ", ""));
        Assert.Equal(0, Match("abcあいうアイウ亜伊宇ｱｲｳ", "abc"));
        Assert.Equal(1, Match("abcあいうアイウ亜伊宇ｱｲｳ", "bcあ"));
        Assert.Equal(2, Match("abcあいうアイウ亜伊宇ｱｲｳ", "cあい"));
        Assert.Equal(3, Match("abcあいうアイウ亜伊宇ｱｲｳ", "あいう"));
        Assert.Equal(4, Match("abcあいうアイウ亜伊宇ｱｲｳ", "いうア"));
        Assert.Equal(5, Match("abcあいうアイウ亜伊宇ｱｲｳ", "うアイ"));
        Assert.Equal(6, Match("abcあいうアイウ亜伊宇ｱｲｳ", "アイウ"));
        Assert.Equal(7, Match("abcあいうアイウ亜伊宇ｱｲｳ", "イウ亜"));
        Assert.Equal(8, Match("abcあいうアイウ亜伊宇ｱｲｳ", "ウ亜伊"));
        Assert.Equal(9, Match("abcあいうアイウ亜伊宇ｱｲｳ", "亜伊宇"));
        Assert.Equal(10, Match("abcあいうアイウ亜伊宇ｱｲｳ", "伊宇ｱ"));
        Assert.Equal(11, Match("abcあいうアイウ亜伊宇ｱｲｳ", "宇ｱｲ"));
        Assert.Equal(12, Match("abcあいうアイウ亜伊宇ｱｲｳ", "ｱｲｳ"));
        Assert.Equal(13, Match("abcあいうアイウ亜伊宇ｱｲｳ", "ｲｳ"));
        Assert.Equal(14, Match("abcあいうアイウ亜伊宇ｱｲｳ", "ｳ"));
    }

    [Fact]
    public void IndexOfPerfomanceTestNormal()
    {
        Assert.Equal(993, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbx".IndexOf("bbbbbbbx", StringComparison.Ordinal));
    }

    [Fact]
    public void IndexOfPerfomanceTestBM()
    {
        Assert.Equal(993, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbx".IndexOf(new BoyerMooreTable("bbbbbbbx")));
    }
}
