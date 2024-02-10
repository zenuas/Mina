using Microsoft.Data.Sqlite;
using Mina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Mina.Test;

public class ObjectMapper_DataReaderTest
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

    public class NullableData
    {
        public int? Prop;
        public long? Field;
    }

    public class DateData
    {
        public DateTime Date;
        public DateTime? DateOrNull;
    }

    public SqliteConnection con;

    public ObjectMapper_DataReaderTest()
    {
        SQLitePCL.Batteries.Init();
        con = new("Data Source=:memory:");

        con.Open();
        using var command = con.CreateCommand();

        command.CommandText = "CREATE TABLE Test3x3(Prop INT, Method VARCHAR(100), Field INT)";
        _ = command.ExecuteNonQuery();
        for (var i = 0; i < 3; i++)
        {
            command.CommandText = $"INSERT INTO Test3x3(Prop, Method, Field) VALUES ({123 + i}, 'test{i}', {456 + i})";
            _ = command.ExecuteNonQuery();
        }

        command.CommandText = "CREATE TABLE Test4x3(Prop INT, Method VARCHAR(100), Field INT, Dummy INT)";
        _ = command.ExecuteNonQuery();
        for (var i = 0; i < 3; i++)
        {
            command.CommandText = $"INSERT INTO Test4x3(Prop, Method, Field, Dummy) VALUES ({123 + i}, 'test{i}', {456 + i}, {789 + i})";
            _ = command.ExecuteNonQuery();
        }

        command.CommandText = "CREATE TABLE Test2Null(Prop INT, Field INT)";
        _ = command.ExecuteNonQuery();
        for (var i = 0; i < 2; i++)
        {
            command.CommandText = $"INSERT INTO Test2Null(Prop, Field) VALUES ({(i == 0 ? "null" : 123 + i)}, {(i != 0 ? "null" : 456 + i)})";
            _ = command.ExecuteNonQuery();
        }

        command.CommandText = "CREATE TABLE Date2Null(Date datetime not null, DateOrNull datetime)";
        _ = command.ExecuteNonQuery();
        for (var i = 0; i < 2; i++)
        {
            command.CommandText = $"INSERT INTO Date2Null(Date, DateOrNull) VALUES ('2000/01/1{i}', {(i != 0 ? "null" : "'2001/02/03'")})";
            _ = command.ExecuteNonQuery();
        }
    }

    [Fact]
    public void ToClass()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test3x3 ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<FieldData>(reader);
        var d = f(reader).ToArray();
        Assert.Equal(d.Length, 3);

        Assert.Equal(d[0].Prop, 123);
        Assert.Equal(d[0].Method, "test0");
        Assert.Equal(d[0].Field, 456);

        Assert.Equal(d[1].Prop, 124);
        Assert.Equal(d[1].Method, "test1");
        Assert.Equal(d[1].Field, 457);

        Assert.Equal(d[2].Prop, 125);
        Assert.Equal(d[2].Method, "test2");
        Assert.Equal(d[2].Field, 458);
    }

    [Fact]
    public void ToStruct()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test3x3 ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<StructData>(reader);
        var d = f(reader).ToArray();
        Assert.Equal(d.Length, 3);

        Assert.Equal(d[0].Prop, 123);
        Assert.Equal(d[0].Method, "test0");
        Assert.Equal(d[0].Field, 456);

        Assert.Equal(d[1].Prop, 124);
        Assert.Equal(d[1].Method, "test1");
        Assert.Equal(d[1].Field, 457);

        Assert.Equal(d[2].Prop, 125);
        Assert.Equal(d[2].Method, "test2");
        Assert.Equal(d[2].Field, 458);
    }

    [Fact]
    public void FilterMap()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test3x3 ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<FieldData>(reader, ["Prop", "Method"]);
        var d = f(reader).ToArray();
        Assert.Equal(d.Length, 3);

        Assert.Equal(d[0].Prop, 123);
        Assert.Equal(d[0].Method, "test0");
        Assert.Equal(d[0].Field, 2);

        Assert.Equal(d[1].Prop, 124);
        Assert.Equal(d[1].Method, "test1");
        Assert.Equal(d[1].Field, 2);

        Assert.Equal(d[2].Prop, 125);
        Assert.Equal(d[2].Method, "test2");
        Assert.Equal(d[2].Field, 2);
    }

    [Fact]
    public void SelectMap()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test3x3 ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<FieldData>(reader, new Dictionary<string, string>() {
            { "Prop", "Field" },
            { "Method", "Method" },
            { "Field", "Prop" },
        });
        var d = f(reader).ToArray();
        Assert.Equal(d.Length, 3);

        Assert.Equal(d[0].Prop, 456);
        Assert.Equal(d[0].Method, "test0");
        Assert.Equal(d[0].Field, 123);

        Assert.Equal(d[1].Prop, 457);
        Assert.Equal(d[1].Method, "test1");
        Assert.Equal(d[1].Field, 124);

        Assert.Equal(d[2].Prop, 458);
        Assert.Equal(d[2].Method, "test2");
        Assert.Equal(d[2].Field, 125);
    }

    [Fact]
    public void Column4ToField3()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test4x3 ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<FieldData>(reader);
        var d = f(reader).ToArray();
        Assert.Equal(d.Length, 3);

        Assert.Equal(d[0].Prop, 123);
        Assert.Equal(d[0].Method, "test0");
        Assert.Equal(d[0].Field, 456);

        Assert.Equal(d[1].Prop, 124);
        Assert.Equal(d[1].Method, "test1");
        Assert.Equal(d[1].Field, 457);

        Assert.Equal(d[2].Prop, 125);
        Assert.Equal(d[2].Method, "test2");
        Assert.Equal(d[2].Field, 458);
    }

    [Fact]
    public void Nullable()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test2Null ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<NullableData>(reader);
        var d = f(reader).ToArray();
        Assert.Equal(d.Length, 2);

        Assert.Null(d[0].Prop);
        Assert.Equal(d[0].Field, 456);

        Assert.Equal(d[1].Prop, 124);
        Assert.Null(d[1].Field);
    }

    [Fact]
    public void DateOrNull()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Date2Null ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<DateData>(reader);
        var d = f(reader).ToArray();
        Assert.Equal(d.Length, 2);

        Assert.Equal(d[0].Date, DateTime.Parse("2000/01/10"));
        Assert.Equal(d[0].DateOrNull, DateTime.Parse("2001/02/03"));

        Assert.Equal(d[1].Date, DateTime.Parse("2000/01/11"));
        Assert.Null(d[1].DateOrNull);
    }

    [Fact]
    public void Error()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test4x3 ORDER BY 1";
        using var reader = command.ExecuteReader();
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => ObjectMapper.CreateMapper<FieldData>(reader, ["Foo"]));
    }
}
