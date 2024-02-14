using Mina.Extension;
using Mina.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mina.Mapper;

public static class DataTableMapper
{
    public static Dictionary<string, string> DefaultMapping<T>(DataTable table) => table.Columns
        .GetIterator()
        .OfType<DataColumn>()
        .Where(x => ILGenerators.WhenStoreAnyPropertyType<T>(x.ColumnName) is { })
        .ToDictionary(x => x.ColumnName, x => x.ColumnName);

    public static Func<DataRow, T> CreateMapper<T>(DataTable table) => CreateMapper<T>(table, DefaultMapping<T>(table));

    public static Func<DataRow, T> CreateMapper<T>(DataTable table, IEnumerable<string> map) => CreateMapper<T>(table, map.ToDictionary(x => x));

    public static Func<DataRow, T> CreateMapper<T>(DataTable table, Dictionary<string, string> map)
    {
        var get_item = typeof(DataRow).GetProperty("Item", [typeof(string)])!.GetMethod!;

        return ObjectMapper.CreateMapper<DataRow, T>(map, (il, name, type) =>
        {
            var load_type = table.Columns[name]!.DataType;

            // stack[top] = (store_type)arg0[from_name];
            il.Ldarg(0);
            il.Ldstr(name);
            il.Call(get_item);
            il.CastViaObject(type, load_type);
        });
    }

    public static Func<IEnumerable<DataRow>, IEnumerable<T>> CreateEnumerableMapper<T>(DataTable table) => CreateEnumerableMapper<T>(table, DefaultMapping<T>(table));

    public static Func<IEnumerable<DataRow>, IEnumerable<T>> CreateEnumerableMapper<T>(DataTable table, IEnumerable<string> map) => CreateEnumerableMapper<T>(table, map.ToDictionary(x => x));

    public static Func<IEnumerable<DataRow>, IEnumerable<T>> CreateEnumerableMapper<T>(DataTable table, Dictionary<string, string> map)
    {
        var f = CreateMapper<T>(table, map);
        return (xs) => xs.Select(f);
    }
}
