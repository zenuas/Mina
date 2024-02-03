using Mina.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace Mina.Test;

public class ObjectMapper_DataTableTest
{
    public class FieldData
    {
        public int Prop = 1;
        public string Method = "";
        public long Field = 2;
    }

    public struct StructData
    {
        public int Prop;
        public string Method;
        public long Field;
    }

    public DataTable table3x3 = new();
    public DataTable table4x3 = new();

    public ObjectMapper_DataTableTest()
    {
        table3x3.Columns.Add("Prop", typeof(int));
        table3x3.Columns.Add("Method", typeof(string));
        table3x3.Columns.Add("Field", typeof(long));

        for (var i = 0; i < 3; i++)
        {
            var row = table3x3.NewRow();
            row["Prop"] = 123 + i;
            row["Method"] = $"test{i}";
            row["Field"] = 456 + i;
            table3x3.Rows.Add(row);
        }

        table4x3.Columns.Add("Prop", typeof(int));
        table4x3.Columns.Add("Method", typeof(string));
        table4x3.Columns.Add("Field", typeof(long));
        table4x3.Columns.Add("Dummy", typeof(short));

        for (var i = 0; i < 3; i++)
        {
            var row = table4x3.NewRow();
            row["Prop"] = 123 + i;
            row["Method"] = $"test{i}";
            row["Field"] = 456 + i;
            row["Dummy"] = 789 + i;
            table4x3.Rows.Add(row);
        }
    }

    [Fact]
    public void CreateMapperDataTable()
    {
        var f = ObjectMapper.CreateMapper<FieldData>(table3x3);
        var d1 = f(table3x3.Rows[0]);
        Assert.Equal(d1.Prop, 123);
        Assert.Equal(d1.Method, "test0");
        Assert.Equal(d1.Field, 456);

        var d2 = f(table3x3.Rows[1]);
        Assert.Equal(d2.Prop, 124);
        Assert.Equal(d2.Method, "test1");
        Assert.Equal(d2.Field, 457);

        var d3 = f(table3x3.Rows[2]);
        Assert.Equal(d3.Prop, 125);
        Assert.Equal(d3.Method, "test2");
        Assert.Equal(d3.Field, 458);
    }

    [Fact]
    public void CreateMapperDataTableToStruct()
    {
        var f = ObjectMapper.CreateMapper<StructData>(table3x3);
        var d1 = f(table3x3.Rows[0]);
        Assert.Equal(d1.Prop, 123);
        Assert.Equal(d1.Method, "test0");
        Assert.Equal(d1.Field, 456);

        var d2 = f(table3x3.Rows[1]);
        Assert.Equal(d2.Prop, 124);
        Assert.Equal(d2.Method, "test1");
        Assert.Equal(d2.Field, 457);

        var d3 = f(table3x3.Rows[2]);
        Assert.Equal(d3.Prop, 125);
        Assert.Equal(d3.Method, "test2");
        Assert.Equal(d3.Field, 458);
    }

    [Fact]
    public void CreateMapperDataTable_FilterMap()
    {
        var f = ObjectMapper.CreateMapper<FieldData>(table3x3, ["Prop", "Method"]);
        var d1 = f(table3x3.Rows[0]);
        Assert.Equal(d1.Prop, 123);
        Assert.Equal(d1.Method, "test0");
        Assert.Equal(d1.Field, 2);

        var d2 = f(table3x3.Rows[1]);
        Assert.Equal(d2.Prop, 124);
        Assert.Equal(d2.Method, "test1");
        Assert.Equal(d2.Field, 2);

        var d3 = f(table3x3.Rows[2]);
        Assert.Equal(d3.Prop, 125);
        Assert.Equal(d3.Method, "test2");
        Assert.Equal(d3.Field, 2);
    }

    [Fact]
    public void CreateMapperDataTable_SelectMap()
    {
        var f = ObjectMapper.CreateMapper<FieldData>(table3x3, new Dictionary<string, string>() {
            { "Prop", "Field" },
            { "Method", "Method" },
            { "Field", "Prop" }
        });
        var d1 = f(table3x3.Rows[0]);
        Assert.Equal(d1.Prop, 456);
        Assert.Equal(d1.Method, "test0");
        Assert.Equal(d1.Field, 123);

        var d2 = f(table3x3.Rows[1]);
        Assert.Equal(d2.Prop, 457);
        Assert.Equal(d2.Method, "test1");
        Assert.Equal(d2.Field, 124);

        var d3 = f(table3x3.Rows[2]);
        Assert.Equal(d3.Prop, 458);
        Assert.Equal(d3.Method, "test2");
        Assert.Equal(d3.Field, 125);
    }

    [Fact]
    public void CreateMapperDataTable_4ColumnTo3Field()
    {
        var f = ObjectMapper.CreateMapper<FieldData>(table4x3);
        var d1 = f(table4x3.Rows[0]);
        Assert.Equal(d1.Prop, 123);
        Assert.Equal(d1.Method, "test0");
        Assert.Equal(d1.Field, 456);

        var d2 = f(table4x3.Rows[1]);
        Assert.Equal(d2.Prop, 124);
        Assert.Equal(d2.Method, "test1");
        Assert.Equal(d2.Field, 457);

        var d3 = f(table4x3.Rows[2]);
        Assert.Equal(d3.Prop, 125);
        Assert.Equal(d3.Method, "test2");
        Assert.Equal(d3.Field, 458);
    }

    [Fact]
    public void CreateMapperDataTable_Error()
    {
        _ = Assert.Throws<NullReferenceException>(() => ObjectMapper.CreateMapper<FieldData>(table3x3, ["Foo"]));
    }
}
