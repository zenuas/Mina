using System.Collections;
using System.Collections.Generic;

namespace Mina.Data;

public class AtLeast3<T> : IReadOnlyList<T>
{
    public required T First { get; init; }
    public required T Second { get; init; }
    public required T Third { get; init; }
    public required IReadOnlyList<T> Rest { get; init; }
    public int Count => Rest.Count + 3;

    public T this[int index] => index switch
    {
        0 => First,
        1 => Second,
        2 => Third,
        _ => Rest[index - 3]
    };

    public IEnumerator<T> GetEnumerator()
    {
        yield return First;
        yield return Second;
        yield return Third;
        foreach (var x in Rest) yield return x;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static AtLeast3<T> Create(T first, T second, T third, IReadOnlyList<T> rest) => new() { First = first, Second = second, Third = third, Rest = rest };

    public static AtLeast3<T> Create(T first, T second, T third, params T[] rest) => new() { First = first, Second = second, Third = third, Rest = rest };
}
