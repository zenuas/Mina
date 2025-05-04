using BenchmarkDotNet.Attributes;
using Mina.Data;
using nietras.SeparatedValues;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Mina.Benchmark;

[MemoryDiagnoser]
public class CsvReaderBench
{
    [Benchmark]
    public List<Person2> CsvReaderMapper_2x100()
    {
        return [.. CsvReader.ReadMapperWithHeader<Person2>(new StringReader(CsvData))];
    }

    [Benchmark]
    public Person2[] CsvReader_2x100()
    {
        var reader = new StringReader(CsvData);
        _ = CsvReader.TryReadFields(reader);

        var result = new Person2[100];
        var index = 0;
        while (true)
        {
            var fields = CsvReader.TryReadFields(reader);
            if (fields is null) break;
            var item = new Person2
            {
                Name = fields[0],
                Age = int.Parse(fields[1]),
            };
            result[index] = item;
            index++;
        }
        return result;
    }

    [Benchmark]
    public List<Person2> CsvHelper_2x100()
    {
        using var reader = new CsvHelper.CsvReader(new StringReader(CsvData), CultureInfo.InvariantCulture);
        return [.. reader.GetRecords<Person2>()];
    }

    [Benchmark]
    public Person2[] Sep_2x100()
    {
        using var reader = Sep.Reader().From(CsvDataUtf8Bytes);
        var result = new Person2[100];

        var index = 0;
        foreach (var row in reader)
        {
            var item = new Person2
            {
                Name = row[0].Parse<string>(),
                Age = row[1].Parse<int>(),
            };
            result[index] = item;
            index++;
        }

        return result;
    }

    public record Person2
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }

    public record Person3
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public DateTime Day { get; set; }
    }


    public static readonly string CsvData =
@"Name,Age,Day
Lauren Wilkins,42,2000/01/01
Victor Rees,43,2000/01/02
Chloe McLean,44,2000/01/03
Warren Dowd,45,2000/01/04
Sonia Berry,46,2000/01/05
Carolyn Parsons,47,2000/01/06
Emma Peake,48,2000/01/07
Jane Russell,49,2000/01/08
Sarah Young,50,2000/01/09
Charles Scott,51,2000/01/10
Jack Chapman,52,2000/01/11
Alexander Young,53,2000/01/12
Colin Jones,54,2000/01/13
Donna Martin,55,2000/01/14
Ryan Marshall,56,2000/01/15
Amanda Bailey,57,2000/01/16
Richard Greene,58,2000/01/17
Tracey Mackay,59,2000/01/18
Blake Bell,60,2000/01/19
Jonathan Edmunds,61,2000/01/20
Dorothy Harris,62,2000/01/21
Una Fisher,63,2000/01/22
Gordon Bond,64,2000/01/23
Nicholas Buckland,65,2000/01/24
Cameron Manning,66,2000/01/25
Thomas Baker,67,2000/01/26
Virginia Turner,68,2000/01/27
Keith Anderson,69,2000/01/28
Anne Fisher,70,2000/01/29
Wendy Bailey,71,2000/01/30
Connor Roberts,72,2000/01/31
Joseph Rutherford,73,2000/02/01
Felicity Hamilton,74,2000/02/02
Jasmine McLean,75,2000/02/03
Rebecca Ferguson,76,2000/02/04
Lauren Rees,77,2000/02/05
Heather Lambert,78,2000/02/06
Emma Robertson,79,2000/02/07
Luke Parsons,80,2000/02/08
Joseph Walker,81,2000/02/09
Anna Watson,82,2000/02/10
Jessica Martin,83,2000/02/11
Adrian Blake,84,2000/02/12
Kevin Metcalfe,85,2000/02/13
Sally McGrath,86,2000/02/14
Jasmine Lee,87,2000/02/15
Bernadette Buckland,88,2000/02/16
Bernadette Springer,89,2000/02/17
Steven Rees,90,2000/02/18
Eric Howard,91,2000/02/19
Eric Hunter,92,2000/02/20
Sue Thomson,93,2000/02/21
Joseph Arnold,94,2000/02/22
Tracey Roberts,95,2000/02/23
Elizabeth Black,96,2000/02/24
Theresa Vaughan,97,2000/02/25
Julia Oliver,98,2000/02/26
Brian Skinner,99,2000/02/27
Stephen Mitchell,100,2000/02/28
Jacob Sutherland,101,2000/02/29
Nicola Mitchell,102,2000/03/01
Ryan Lambert,103,2000/03/02
Steven Roberts,104,2000/03/03
Leonard Allan,105,2000/03/04
Jake North,106,2000/03/05
James Morrison,107,2000/03/06
Boris Berry,108,2000/03/07
Jacob White,109,2000/03/08
Ava Sutherland,110,2000/03/09
Jane White,111,2000/03/10
Rachel Cameron,112,2000/03/11
Brandon Wilson,113,2000/03/12
Jessica Graham,114,2000/03/13
Kimberly Randall,115,2000/03/14
Amanda Nash,116,2000/03/15
Owen Hudson,117,2000/03/16
John Roberts,118,2000/03/17
Karen Stewart,119,2000/03/18
Amanda Fisher,120,2000/03/19
Phil Fraser,121,2000/03/20
Carol Ellison,122,2000/03/21
Andrew Dyer,123,2000/03/22
Richard Baker,124,2000/03/23
Jake McGrath,125,2000/03/24
Amelia Ball,126,2000/03/25
Victoria Hodges,127,2000/03/26
Peter Watson,128,2000/03/27
Piers Forsyth,129,2000/03/28
Evan Poole,130,2000/03/29
Jessica Henderson,131,2000/03/30
Deirdre Metcalfe,132,2000/03/31
Frank Russell,133,2000/04/01
Alexander Russell,134,2000/04/02
Brandon Scott,135,2000/04/03
Gordon Fraser,136,2000/04/04
Sean Turner,137,2000/04/05
Kylie Baker,138,2000/04/06
Leah Hudson,139,2000/04/07
Diana Hunter,140,2000/04/08
Isaac May,141,2000/04/09";

    public static readonly byte[] CsvDataUtf8Bytes = Encoding.UTF8.GetBytes(CsvData);
}
