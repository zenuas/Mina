using System;

namespace Mina.Extension;

public static class SafeConvert
{
    public static bool ToBool(object? obj, bool default_value = default) => TryConvert<bool>(obj, out var value) ? value : default_value;
    public static sbyte ToSByte(object? obj, sbyte default_value = default) => TryConvert<sbyte>(obj, out var value) ? value : default_value;
    public static short ToShort(object? obj, short default_value = default) => TryConvert<short>(obj, out var value) ? value : default_value;
    public static int ToInt(object? obj, int default_value = default) => TryConvert<int>(obj, out var value) ? value : default_value;
    public static long ToLong(object? obj, long default_value = default) => TryConvert<long>(obj, out var value) ? value : default_value;
    public static byte ToByte(object? obj, byte default_value = default) => TryConvert<byte>(obj, out var value) ? value : default_value;
    public static ushort ToUShort(object? obj, ushort default_value = default) => TryConvert<ushort>(obj, out var value) ? value : default_value;
    public static uint ToUInt(object? obj, uint default_value = default) => TryConvert<uint>(obj, out var value) ? value : default_value;
    public static ulong ToULong(object? obj, ulong default_value = default) => TryConvert<ulong>(obj, out var value) ? value : default_value;
    public static float ToFloat(object? obj, float default_value = default) => TryConvert<float>(obj, out var value) ? value : default_value;
    public static double ToDouble(object? obj, double default_value = default) => TryConvert<double>(obj, out var value) ? value : default_value;
    public static decimal ToDecimal(object? obj, decimal default_value = default) => TryConvert<decimal>(obj, out var value) ? value : default_value;
    public static DateTime ToDateTime(object? obj, DateTime default_value = default) => TryConvert<DateTime>(obj, out var value) ? value : default_value;
    public static string ToString(object? obj, string default_value = "") => ((Func<string>)(() => obj?.ToString() ?? default_value)).Catch(out var value) is null ? value : default_value;

    public static bool? ToBoolOrNull(object? obj) => TryConvert<bool>(obj, out var value) ? value : null;
    public static sbyte? ToSByteOrNull(object? obj) => TryConvert<sbyte>(obj, out var value) ? value : null;
    public static short? ToShortOrNull(object? obj) => TryConvert<short>(obj, out var value) ? value : null;
    public static int? ToIntOrNull(object? obj) => TryConvert<int>(obj, out var value) ? value : null;
    public static long? ToLongOrNull(object? obj) => TryConvert<long>(obj, out var value) ? value : null;
    public static byte? ToByteOrNull(object? obj) => TryConvert<byte>(obj, out var value) ? value : null;
    public static ushort? ToUShortOrNull(object? obj) => TryConvert<ushort>(obj, out var value) ? value : null;
    public static uint? ToUIntOrNull(object? obj) => TryConvert<uint>(obj, out var value) ? value : null;
    public static ulong? ToULongOrNull(object? obj) => TryConvert<ulong>(obj, out var value) ? value : null;
    public static float? ToFloatOrNull(object? obj) => TryConvert<float>(obj, out var value) ? value : null;
    public static double? ToDoubleOrNull(object? obj) => TryConvert<double>(obj, out var value) ? value : null;
    public static decimal? ToDecimalOrNull(object? obj) => TryConvert<decimal>(obj, out var value) ? value : null;
    public static DateTime? ToDateTimeOrNull(object? obj) => TryConvert<DateTime>(obj, out var value) ? value : null;
    public static string? ToStringOrNull(object? obj) => ((Func<string?>)(() => obj?.ToString())).Catch(out var value) is null ? value : null;

    public static bool TryConvert<T>(object? obj, out T value) => ((Func<T>)(() => (T)Convert.ChangeType(obj, typeof(T))!)).Catch(out value) is null;
    public static bool TryParse<T>(string s, out T value) where T : IParsable<T> => T.TryParse(s, null, out value!);
    public static bool TryParse<T>(string s, IFormatProvider? format, out T value) where T : IParsable<T> => T.TryParse(s, format, out value!);
}
