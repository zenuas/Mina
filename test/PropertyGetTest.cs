using Mina.Data;
using Xunit;

namespace Mina.Test;

public class PropertyGetTest
{
    [Fact]
    public void GetSetTest()
    {
        var v = new PropertyGetSet<int> { Value = 10 };
        var w = v;

        Assert.Equal(v.Value, 10);
        Assert.Equal(w.Value, 10);

        v.Value = 20;

        Assert.Equal(v.Value, 20);
        Assert.Equal(w.Value, 20);
    }

    [Fact]
    public void GetInitTest()
    {
        var v = new PropertyGetInit<int> { Value = 10 };
        var w = v;

        Assert.Equal(v.Value, 10);
        Assert.Equal(w.Value, 10);
    }
}
