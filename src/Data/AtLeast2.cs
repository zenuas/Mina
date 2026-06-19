using System.Collections;
using System.Collections.Generic;

namespace Mina.Data;

public class AtLeast2<T> : IReadOnlyList<T>
{
    public required T First { get; init; }
    public required T Second { get; init; }
    public required IReadOnlyList<T> Rest { get; init; }
    public int Count => Rest.Count + 2;

    public T this[int index] => index switch
    {
        0 => First,
        1 => Second,
        _ => Rest[index - 2]
    };

    public IEnumerator<T> GetEnumerator()
    {
        yield return First;
        yield return Second;
        foreach (var x in Rest) yield return x;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static AtLeast2<T> Create(T first, T second, IReadOnlyList<T> rest) => new() { First = first, Second = second, Rest = rest };

    public static AtLeast2<T> Create(T first, T second, params T[] rest) => new() { First = first, Second = second, Rest = rest };
}
