using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.Data.Sqlite;
using Mina.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mina.Benchmark;

public class DataReaderMapperBench
{
    public SqliteConnection con;

    public class TestData
    {
        public int Id { get; set; }
        public string Text1 { get; set; } = "";
        public string Text2 { get; set; } = "";
        public DateTime Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public int Int1 { get; set; }
        public int? Int2 { get; set; }
        public int? Int3 { get; set; }
        public int? Int4 { get; set; }
        public int? Int5 { get; set; }
        public int? Int6 { get; set; }
        public int? Int7 { get; set; }
        public int? Int8 { get; set; }
        public int? Int9 { get; set; }
    }

    public DataReaderMapperBench()
    {
        SQLitePCL.Batteries.Init();
        con = new("Data Source=:memory:");

        con.Open();
        using var command = con.CreateCommand();

        command.CommandText = @"
CREATE TABLE Test
(
	Id int identity primary key,
	Text1 varchar(1000) not null,
	Text2 varchar(1000),
	Date1 datetime not null,
	Date2 datetime,
	Int1 int not null,
	Int2 int,
	Int3 int,
	Int4 int,
	Int5 int,
	Int6 int,
	Int7 int,
	Int8 int,
	Int9 int
)";
        _ = command.ExecuteNonQuery();
        for (var i = 0; i < 5000; i++)
        {
            command.CommandText = $"INSERT INTO Test (Id, Text1, Date1, Int1) VALUES ({i}, 'xyz', '2000/12/31', {i});";
            _ = command.ExecuteNonQuery();
        }
    }

    [Benchmark]
    public void ObjectMapperAll()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = ObjectMapper.CreateMapper<TestData>(reader);
        var xs = f(reader).ToArray();
    }

    public static Func<IDataReader, IEnumerable<TestData>>? CachedObjectMapperAll_cache_ = null;

    [Benchmark]
    public void CachedObjectMapperAll()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = CachedObjectMapperAll_cache_ ??= ObjectMapper.CreateMapper<TestData>(reader);
        var xs = f(reader).ToArray();
    }

    [Benchmark]
    public void DapperAll()
    {
        var xs = con.Query<TestData>($"SELECT * FROM Test ORDER BY 1").ToArray();
    }
}
