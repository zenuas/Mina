using System.Collections;
using System.Collections.Generic;

namespace Mina.Data;

public class AtLeast1<T> : IReadOnlyList<T>
{
    public required T First { get; init; }
    public required IReadOnlyList<T> Rest { get; init; }
    public int Count => Rest.Count + 1;

    public T this[int index] => index switch
    {
        0 => First,
        _ => Rest[index - 1]
    };

    public IEnumerator<T> GetEnumerator()
    {
        yield return First;
        foreach (var x in Rest) yield return x;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static AtLeast1<T> Create(T first, IReadOnlyList<T> rest) => new() { First = first, Rest = rest };

    public static AtLeast1<T> Create(T first, params T[] rest) => new() { First = first, Rest = rest };
}
