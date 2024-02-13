using Mina.Reflections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;

namespace Mina.Mapper;

public static class DataReaderMapper
{
    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader) => CreateMapper<T>(reader, Enumerable.Range(0, reader.FieldCount)
        .Select(x => reader.GetName(x))
        .Where(x => ILGenerators.WhenStoreAnyPropertyType<T>(x) is { })
        .ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader, IEnumerable<string> map) => CreateMapper<T>(reader, map.ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateMapper<T>(IDataReader reader, Dictionary<string, string> map)
    {
        var f = ObjectMapper.CreateMapper<object[], T>(map, (il, name, type) =>
        {
            var index = reader.GetOrdinal(name);
            var load_type = reader.GetFieldType(index);

            // stack[top] = (store_type)arg0[index];
            il.Ldarg(0);
            il.Ldc_I4(index);
            il.Emit(OpCodes.Ldelem_Ref);
            il.CastViaObject(type, load_type);
        });
        static IEnumerable<T> ReadMapper(IDataReader reader, Func<object[], T> mapper)
        {
            var buffer = new object[reader.FieldCount];
            while (reader.Read())
            {
                _ = reader.GetValues(buffer);
                yield return mapper(buffer);
            }
        }
        return (arg) => ReadMapper(arg, f);
    }
}
