﻿using System;
using System.Linq;

namespace Mina.Extension;

public static class Objects
{
    public static R If<T, R>(this T self, Func<T, bool> cond, Func<T, R> then, Func<T, R> else_) => cond(self) ? then(self) : else_(self);

    public static T Then<T>(this T self, Func<T, bool> cond, Func<T, T> then) => cond(self) ? then(self) : self;

    public static T Else<T>(this T self, Func<T, bool> cond, Func<T, T> else_) => cond(self) ? self : else_(self);

    public static T Return<T>(this T self, Action<T> f)
    {
        f(self);
        return self;
    }

    public static R To<T, R>(this T self, Func<T, R> f) => f(self);

    public static bool In<T>(this T self, params T[] args) where T : IEquatable<T> => !args.Where(self.Equals).IsEmpty();

    public static bool InClass<T>(this T self, params T[] args) where T : class => !args.Where(self.Equals).IsEmpty();

    public static bool InStruct<T>(this T self, params T[] args) where T : struct => !args.Where(x => self.Equals(x)).IsEmpty();

    public static T Cast<T>(this object self) => (T)self;

    public static T Try<T>(this T? self) where T : class => self ?? throw new();

    public static T Try<T>(this T? self) where T : struct => self ?? throw new();

    public static Exception? Catch<T>(this Func<T> self, out T value)
    {
        value = default!;
        try
        {
            value = self();
            return null;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
