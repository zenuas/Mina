using Mina.Extensions;
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
}
