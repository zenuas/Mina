﻿using Mina.Extensions;
using System.Collections.Generic;
using System.IO;

namespace Mina.Datas;

public static class CsvReader
{
    public static string[] ReadFields(TextReader input)
    {
        var fields = TryReadFields(input);
        if (fields is { }) return fields;
        throw new EndOfStreamException();
    }

    public static string[]? TryReadFields(TextReader input)
    {
        var fields = new List<string>();
        var c = input.PeekNewLineLF();

        if (c is null) return null;
        if (c == '\n')
        {
            _ = input.ReadNewLineLF();
            return [""];
        }

        while (true)
        {
            fields.Add(c == '"' ? ReadRFC4180EscapedField(input) : ReadRFC4180Field(input));

            c = input.PeekNewLineLF();
            if (c is null)
            {
                break;
            }
            if (c == '\n')
            {
                _ = input.ReadNewLineLF();
                break;
            }
            else if (c == ',')
            {
                _ = input.ReadNewLineLF();
                c = input.PeekNewLineLF();
            }
        }
        return [.. fields];
    }

    public static string ReadRFC4180Field(TextReader input)
    {
        var field = new List<char>();
        while (true)
        {
            var c = input.PeekNewLineLF();
            if (c is null || c == '\n' || c == ',') break;
            field.Add((char)c);
            _ = input.ReadNewLineLF();
        }
        return field.ToStringByChars();
    }

    public static string ReadRFC4180EscapedField(TextReader input)
    {
        if (input.ReadNewLineLF() != '"') throw new();

        var field = new List<char>();
        while (true)
        {
            var c = input.Read();
            if (c < 0) break;
            if (c == '"')
            {
                if (input.Peek() != '"') break;
                _ = input.Read();
            }
            field.Add((char)c);
        }
        return field.ToStringByChars();
    }
}
