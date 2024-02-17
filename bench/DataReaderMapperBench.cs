using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.Data.Sqlite;
using Mina.Mapper;
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
    public void HandAll()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test ORDER BY 1";
        using var reader = command.ExecuteReader();
        var lists = new List<TestData>();
        while (reader.Read())
        {
            var x = new TestData();

            x.Id = reader.GetInt32(0);
            x.Text1 = reader.GetString(1);
            x.Text2 = reader.IsDBNull(2) ? "" : reader.GetString(2);
            x.Date1 = reader.GetDateTime(3);
            x.Date2 = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
            x.Int1 = reader.GetInt32(5);
            x.Int2 = reader.IsDBNull(6) ? null : reader.GetInt32(6);
            x.Int3 = reader.IsDBNull(7) ? null : reader.GetInt32(7);
            x.Int4 = reader.IsDBNull(8) ? null : reader.GetInt32(8);
            x.Int5 = reader.IsDBNull(9) ? null : reader.GetInt32(9);
            x.Int6 = reader.IsDBNull(10) ? null : reader.GetInt32(10);
            x.Int7 = reader.IsDBNull(11) ? null : reader.GetInt32(11);
            x.Int8 = reader.IsDBNull(12) ? null : reader.GetInt32(12);
            x.Int9 = reader.IsDBNull(13) ? null : reader.GetInt32(13);
            lists.Add(x);
        }
    }

    public static Func<IDataReader, IEnumerable<TestData>>? CachedDataReaderMapperAll_cache_ = null;

    [Benchmark]
    public void CachedDataReaderMapperAll()
    {
        using var command = con.CreateCommand();
        command.CommandText = $"SELECT * FROM Test ORDER BY 1";
        using var reader = command.ExecuteReader();
        var f = CachedDataReaderMapperAll_cache_ ??= DataReaderMapper.CreateEnumerableMapper<TestData>(reader);
        var xs = f(reader).ToArray();
    }

    [Benchmark]
    public void DapperAll()
    {
        var xs = con.Query<TestData>($"SELECT * FROM Test ORDER BY 1").ToArray();
    }
}
