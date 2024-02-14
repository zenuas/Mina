using Mina.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;

namespace Mina.Mapper;

public static class DataReaderMapper
{
    public static Dictionary<string, string> DefaultMapping<T>(IDataReader reader) => Enumerable.Range(0, reader.FieldCount)
        .Select(reader.GetName)
        .Where(x => ILGenerators.WhenStoreAnyPropertyType<T>(x) is { })
        .ToDictionary(x => x);

    public static Func<object[], T> CreateMapperBase<T>(IDataReader reader, Dictionary<string, string> map) => ObjectMapper.CreateMapper<object[], T>(map, (il, name, type) =>
    {
        var index = reader.GetOrdinal(name);
        var load_type = reader.GetFieldType(index);

        // stack[top] = (store_type)arg0[index];
        il.Ldarg(0);
        il.Ldc_I4(index);
        il.Emit(OpCodes.Ldelem_Ref);
        il.CastViaObject(type, load_type);
    });

    public static Func<IDataReader, T> CreateMapper<T>(IDataReader reader) => CreateMapper<T>(reader, DefaultMapping<T>(reader));

    public static Func<IDataReader, T> CreateMapper<T>(IDataReader reader, IEnumerable<string> map) => CreateMapper<T>(reader, map.ToDictionary(x => x));

    public static Func<IDataReader, T> CreateMapper<T>(IDataReader reader, Dictionary<string, string> map)
    {
        var f = CreateMapperBase<T>(reader, map);
        return (reader) =>
        {
            var buffer = new object[reader.FieldCount];
            _ = reader.GetValues(buffer);
            return f(buffer);
        };
    }

    public static Func<IDataReader, IEnumerable<T>> CreateEnumerableMapper<T>(IDataReader reader) => CreateEnumerableMapper<T>(reader, DefaultMapping<T>(reader));

    public static Func<IDataReader, IEnumerable<T>> CreateEnumerableMapper<T>(IDataReader reader, IEnumerable<string> map) => CreateEnumerableMapper<T>(reader, map.ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateEnumerableMapper<T>(IDataReader reader, Dictionary<string, string> map)
    {
        static IEnumerable<T> ReadMapper(IDataReader reader, Func<object[], T> mapper)
        {
            var buffer = new object[reader.FieldCount];
            while (reader.Read())
            {
                _ = reader.GetValues(buffer);
                yield return mapper(buffer);
            }
        }
        var f = CreateMapperBase<T>(reader, map);
        return (xs) => ReadMapper(xs, f);
    }
}
