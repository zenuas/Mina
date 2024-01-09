using Mina.Extensions;
using System;
using Xunit;

namespace Mina.Test;

public class WildcardTest
{
    [Fact]
    public void NullTest()
    {
        Assert.True("".IsWildcardMatch(""));
        Assert.True("".IsWildcardMatch("*"));
        Assert.False("a".IsWildcardMatch(""));
        Assert.False("".IsWildcardMatch("a"));
    }

    [Fact]
    public void StringTest()
    {
        Assert.True("a".IsWildcardMatch("a"));
        Assert.True("aa".IsWildcardMatch("aa"));
        Assert.True("aaa".IsWildcardMatch("aaa"));

        Assert.False("a".IsWildcardMatch("b"));
        Assert.False("aa".IsWildcardMatch("ab"));
        Assert.False("aaa".IsWildcardMatch("aab"));
    }

    [Fact]
    public void Wild1Test()
    {
        Assert.True("a".IsWildcardMatch("?"));
        Assert.True("aa".IsWildcardMatch("a?"));
        Assert.True("aaa".IsWildcardMatch("a?a"));

        Assert.True("b".IsWildcardMatch("?"));
        Assert.True("ab".IsWildcardMatch("a?"));
        Assert.True("aba".IsWildcardMatch("a?a"));

        Assert.False("aa".IsWildcardMatch("?b"));
        Assert.False("aaa".IsWildcardMatch("a?b"));
    }

    [Fact]
    public void Wild2Test()
    {
        Assert.True("a".IsWildcardMatch("*"));
        Assert.True("aa".IsWildcardMatch("a*"));
        Assert.True("aaa".IsWildcardMatch("a*a"));
        Assert.True("aaaa".IsWildcardMatch("a*a"));
        Assert.True("aaaaa".IsWildcardMatch("a*a"));

        Assert.True("b".IsWildcardMatch("*"));
        Assert.True("ab".IsWildcardMatch("a*"));
        Assert.True("aba".IsWildcardMatch("a*a"));
        Assert.True("abca".IsWildcardMatch("a*a"));
        Assert.True("abcda".IsWildcardMatch("a*a"));

        Assert.False("aa".IsWildcardMatch("*b"));
        Assert.False("aaa".IsWildcardMatch("a*b"));
        Assert.False("aba".IsWildcardMatch("a*b"));
    }

    [Fact]
    public void Wild3Test()
    {
        Assert.True("abc xyz".IsWildcardMatch("*"));
        Assert.True("abc xyz".IsWildcardMatch("ab*yz"));
        Assert.True("abc xyz".IsWildcardMatch("a* *z"));
        Assert.True("abc xyz".IsWildcardMatch("*c x*"));

        Assert.False("abc xyz".IsWildcardMatch("ab*yw"));
        Assert.False("abc xyz".IsWildcardMatch("a*-*z"));
        Assert.False("abc xyz".IsWildcardMatch("*c-x*"));
    }

    [Fact]
    public void WildEndTest()
    {
        Assert.True("".IsWildcardMatch("**"));
        Assert.True("a".IsWildcardMatch("**"));
        Assert.True("a".IsWildcardMatch("***"));
        Assert.True("aa".IsWildcardMatch("**"));
        Assert.True("aa".IsWildcardMatch("***"));
        Assert.True("aaa".IsWildcardMatch("**"));
        Assert.True("aaa".IsWildcardMatch("***"));
        Assert.True("img001.jpg".IsWildcardMatch("img*.jpg"));
        Assert.True("img001.002.jpg".IsWildcardMatch("img*.*.jpg"));
        Assert.True("img001..jpg".IsWildcardMatch("img*.*.jpg"));

        Assert.False("".IsWildcardMatch("**x"));
        Assert.False("a".IsWildcardMatch("**x"));
        Assert.False("a".IsWildcardMatch("***x"));
        Assert.False("aa".IsWildcardMatch("**x"));
        Assert.False("aa".IsWildcardMatch("***x"));
        Assert.False("aaa".IsWildcardMatch("**x"));
        Assert.False("aaa".IsWildcardMatch("***x"));
        Assert.False("img001.jpg".IsWildcardMatch("img*.png"));
        Assert.False("img001.002.jpg".IsWildcardMatch("img*.*.png"));
        Assert.False("img001..jpg".IsWildcardMatch("img*.*.png"));
    }
}
