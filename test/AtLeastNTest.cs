using Mina.Data;
using System.Linq;
using Xunit;

namespace Mina.Test;

public class AtLeastNTest
{
    [Fact]
    public void New()
    {
        var a1 = new AtLeast1<int>() { First = 11, Rest = [] };
        var a2 = new AtLeast2<int>() { First = 21, Second = 22, Rest = [] };
        var a3 = new AtLeast3<int>() { First = 31, Second = 32, Third = 33, Rest = [] };

        Assert.Equal(a1.Count, 1);
        Assert.Equal(a2.Count, 2);
        Assert.Equal(a3.Count, 3);

        Assert.Equal(a1[0], 11);

        Assert.Equal(a2[0], 21);
        Assert.Equal(a2[1], 22);

        Assert.Equal(a3[0], 31);
        Assert.Equal(a3[1], 32);
        Assert.Equal(a3[2], 33);

        var b1 = new AtLeast1<int>() { First = 11, Rest = [12] };
        var b2 = new AtLeast2<int>() { First = 21, Second = 22, Rest = [23] };
        var b3 = new AtLeast3<int>() { First = 31, Second = 32, Third = 33, Rest = [34] };

        Assert.Equal(b1.Count, 2);
        Assert.Equal(b2.Count, 3);
        Assert.Equal(b3.Count, 4);

        Assert.Equal(b1[0], 11);
        Assert.Equal(b1[1], 12);

        Assert.Equal(b2[0], 21);
        Assert.Equal(b2[1], 22);
        Assert.Equal(b2[2], 23);

        Assert.Equal(b3[0], 31);
        Assert.Equal(b3[1], 32);
        Assert.Equal(b3[2], 33);
        Assert.Equal(b3[3], 34);

        var c1 = new AtLeast1<int>() { First = 11, Rest = new int[] { 12, 13 }.Select(x => x + 1).ToList() };
        var c2 = new AtLeast2<int>() { First = 21, Second = 22, Rest = new int[] { 23, 24 }.Select(x => x + 1).ToList() };
        var c3 = new AtLeast3<int>() { First = 31, Second = 32, Third = 33, Rest = new int[] { 34, 35 }.Select(x => x + 1).ToList() };

        Assert.Equal(c1.Count, 3);
        Assert.Equal(c2.Count, 4);
        Assert.Equal(c3.Count, 5);

        Assert.Equal(c1[0], 11);
        Assert.Equal(c1[1], 13);
        Assert.Equal(c1[2], 14);

        Assert.Equal(c2[0], 21);
        Assert.Equal(c2[1], 22);
        Assert.Equal(c2[2], 24);
        Assert.Equal(c2[3], 25);

        Assert.Equal(c3[0], 31);
        Assert.Equal(c3[1], 32);
        Assert.Equal(c3[2], 33);
        Assert.Equal(c3[3], 35);
        Assert.Equal(c3[4], 36);

        var i = 30;
        foreach (var x in b3)
        {
            Assert.Equal(x, ++i);
        }
    }

    [Fact]
    public void Create()
    {
        var a1 = AtLeast1<int>.Create(11);
        var a2 = AtLeast2<int>.Create(21, 22);
        var a3 = AtLeast3<int>.Create(31, 32, 33);

        Assert.Equal(a1.Count, 1);
        Assert.Equal(a2.Count, 2);
        Assert.Equal(a3.Count, 3);

        Assert.Equal(a1[0], 11);

        Assert.Equal(a2[0], 21);
        Assert.Equal(a2[1], 22);

        Assert.Equal(a3[0], 31);
        Assert.Equal(a3[1], 32);
        Assert.Equal(a3[2], 33);

        var b1 = AtLeast1<int>.Create(11, 12);
        var b2 = AtLeast2<int>.Create(21, 22, 23);
        var b3 = AtLeast3<int>.Create(31, 32, 33, 34);

        Assert.Equal(b1.Count, 2);
        Assert.Equal(b2.Count, 3);
        Assert.Equal(b3.Count, 4);

        Assert.Equal(b1[0], 11);
        Assert.Equal(b1[1], 12);

        Assert.Equal(b2[0], 21);
        Assert.Equal(b2[1], 22);
        Assert.Equal(b2[2], 23);

        Assert.Equal(b3[0], 31);
        Assert.Equal(b3[1], 32);
        Assert.Equal(b3[2], 33);
        Assert.Equal(b3[3], 34);

        var c1 = AtLeast1<int>.Create(11, new int[] { 12, 13 }.Select(x => x + 1).ToList());
        var c2 = AtLeast2<int>.Create(21, 22, new int[] { 23, 24 }.Select(x => x + 1).ToList());
        var c3 = AtLeast3<int>.Create(31, 32, 33, new int[] { 34, 35 }.Select(x => x + 1).ToList());

        Assert.Equal(c1.Count, 3);
        Assert.Equal(c2.Count, 4);
        Assert.Equal(c3.Count, 5);

        Assert.Equal(c1[0], 11);
        Assert.Equal(c1[1], 13);
        Assert.Equal(c1[2], 14);

        Assert.Equal(c2[0], 21);
        Assert.Equal(c2[1], 22);
        Assert.Equal(c2[2], 24);
        Assert.Equal(c2[3], 25);

        Assert.Equal(c3[0], 31);
        Assert.Equal(c3[1], 32);
        Assert.Equal(c3[2], 33);
        Assert.Equal(c3[3], 35);
        Assert.Equal(c3[4], 36);

        var i = 30;
        foreach (var x in b3)
        {
            Assert.Equal(x, ++i);
        }
    }
}
