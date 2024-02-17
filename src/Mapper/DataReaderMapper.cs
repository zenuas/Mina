using Mina.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Mina.Mapper;

public static class DataReaderMapper
{
    public static Dictionary<string, string> DefaultMapping<T>(IDataReader reader) => Enumerable.Range(0, reader.FieldCount)
        .Select(reader.GetName)
        .Where(x => ILGenerators.WhenStoreAnyPropertyType<T>(x) is { })
        .ToDictionary(x => x);

    public static MethodInfo GetGetFieldMethod(Type t) => t switch
    {
        Type a when a == typeof(char) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetChar))!,
        Type a when a == typeof(bool) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetBoolean))!,
        Type a when a == typeof(byte) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetByte))!,
        Type a when a == typeof(short) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt16))!,
        Type a when a == typeof(int) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt32))!,
        Type a when a == typeof(long) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt64))!,
        Type a when a == typeof(float) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetFloat))!,
        Type a when a == typeof(double) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDouble))!,
        Type a when a == typeof(decimal) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDecimal))!,
        Type a when a == typeof(string) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetString))!,
        Type a when a == typeof(DateTime) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDateTime))!,
        Type a when a == typeof(Guid) => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetGuid))!,
        _ => typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetValue))!,
    };

    public static Func<IDataReader, T> CreateMapper<T>(IDataReader reader) => CreateMapper<T>(reader, DefaultMapping<T>(reader));

    public static Func<IDataReader, T> CreateMapper<T>(IDataReader reader, IEnumerable<string> map) => CreateMapper<T>(reader, map.ToDictionary(x => x));

    public static Func<IDataReader, T> CreateMapper<T>(IDataReader reader, Dictionary<string, string> map) => ObjectMapper.CreateMapper<IDataReader, T>(map, (il, name, type) =>
    {
        var index = reader.GetOrdinal(name);
        var load_type = reader.GetFieldType(index);

        if (!type.IsValueType || Nullable.GetUnderlyingType(type) is { })
        {
            // if (!arg0.IsDBNull(index))
            il.Ldarg(0);
            il.Ldc_I4(index);
            il.Call(typeof(IDataRecord).GetMethod(nameof(IDataRecord.IsDBNull))!);
            var else_label = il.IfFalseElseGoto_S();

            // then: stack[top] = (type)GetGetFieldMethod(t)(index);
            il.Ldarg(0);
            il.Ldc_I4(index);
            il.Call(GetGetFieldMethod(load_type));
            il.AnyCast(type, load_type);
            var endif_label = il.Br_S();

            // else: stack[top] = (type)null;
            il.MarkLabel(else_label);
            il.Emit(OpCodes.Ldnull);
            il.CastViaObject(type, load_type);

            il.MarkLabel(endif_label);
        }
        else
        {
            // stack[top] = (type)GetGetFieldMethod(t)(index);
            il.Ldarg(0);
            il.Ldc_I4(index);
            il.Call(GetGetFieldMethod(load_type));
            il.AnyCast(type, load_type);
        }
    });

    public static Func<IDataReader, IEnumerable<T>> CreateEnumerableMapper<T>(IDataReader reader) => CreateEnumerableMapper<T>(reader, DefaultMapping<T>(reader));

    public static Func<IDataReader, IEnumerable<T>> CreateEnumerableMapper<T>(IDataReader reader, IEnumerable<string> map) => CreateEnumerableMapper<T>(reader, map.ToDictionary(x => x));

    public static Func<IDataReader, IEnumerable<T>> CreateEnumerableMapper<T>(IDataReader reader, Dictionary<string, string> map)
    {
        static IEnumerable<T> EnumerableMapper(IDataReader reader, Func<IDataReader, T> mapper)
        {
            while (reader.Read())
            {
                yield return mapper(reader);
            }
        }
        var f = CreateMapper<T>(reader, map);
        return (xs) => EnumerableMapper(xs, f);
    }
}
