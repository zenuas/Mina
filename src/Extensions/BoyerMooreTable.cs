using System.Collections.Generic;

namespace Mina.Extensions;

public class BoyerMooreTable : Dictionary<char, int>
{
    public string Value { get; init; }

    public BoyerMooreTable(string value)
    {
        Value = value;
        for (var i = 0; i < value.Length; i++)
        {
            this[value[i]] = value.Length - i - 1;
        }
    }
}
