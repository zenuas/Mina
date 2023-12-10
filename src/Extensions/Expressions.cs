using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace Extensions;

public static class Expressions
{
    public static Func<T, T, T> Add<T>() where T : IAdditionOperators<T, T, T> => (a, b) => a + b;

    public static Func<T, T, T> Subtract<T>() where T : ISubtractionOperators<T, T, T> => (a, b) => a - b;

    public static Func<T, T, T> Multiply<T>() where T : IMultiplyOperators<T, T, T> => (a, b) => a * b;

    public static Func<T, T, T> Divide<T>() where T : IDivisionOperators<T, T, T> => (a, b) => a / b;

    public static Func<T, T, T> Modulo<T>() where T : IModulusOperators<T, T, T> => (a, b) => a % b;

    public static Func<T, T, T> LeftShift<T>() where T : IShiftOperators<T, T, T> => (a, b) => a << b;

    public static Func<T, T, T> RightShift<T>() where T : IShiftOperators<T, T, T> => (a, b) => a >> b;

    public static Func<T, R> GetProperty<T, R>(string name) => GetFunction<T, R>(typeof(T).GetProperty(name)!.GetMethod!);

    public static Action<T, A> SetProperty<T, A>(string name) => GetAction<T, A>(typeof(T).GetProperty(name)!.SetMethod!);

    public static Func<T, R> GetFunction<T, R>(string name) => GetFunction<T, R>(typeof(T).GetMethod(name)!);

    public static Func<T, A, R> GetFunction<T, A, R>(string name) => GetFunction<T, A, R>(typeof(T).GetMethod(name)!);

    public static Func<T, A1, A2, R> GetFunction<T, A1, A2, R>(string name) => GetFunction<T, A1, A2, R>(typeof(T).GetMethod(name)!);

    public static Action<T> GetAction<T>(string name) => GetAction<T>(typeof(T).GetMethod(name)!);

    public static Action<T, A> GetAction<T, A>(string name) => GetAction<T, A>(typeof(T).GetMethod(name)!);

    public static Action<T, A1, A2> GetAction<T, A1, A2>(string name) => GetAction<T, A1, A2>(typeof(T).GetMethod(name)!);

    public static Func<T, R> GetFunction<T, R>(MethodInfo method)
    {
        var receiver = Expression.Parameter(typeof(T));
        return Expression.Lambda<Func<T, R>>(
                ExpressionConvert(Expression.Call(receiver, method), typeof(R), method.ReturnParameter.ParameterType),
                receiver
            ).Compile();
    }

    public static Func<T, A, R> GetFunction<T, A, R>(MethodInfo method)
    {
        var receiver = Expression.Parameter(typeof(T));
        var parameters = method.GetParameters();
        var arg1 = Expression.Variable(parameters[0].ParameterType);
        var param1 = Expression.Parameter(typeof(A));
        return Expression.Lambda<Func<T, A, R>>(
                Expression.Block(
                        typeof(R),
                        [arg1],
                        ExpressionConvertAssign(arg1, param1, parameters[0].ParameterType, typeof(A)),
                        ExpressionConvert(Expression.Call(receiver, method, arg1), typeof(R), method.ReturnParameter.ParameterType)
                    ),
                receiver,
                param1
            ).Compile();
    }

    public static Func<T, A1, A2, R> GetFunction<T, A1, A2, R>(MethodInfo method)
    {
        var receiver = Expression.Parameter(typeof(T));
        var parameters = method.GetParameters();
        var arg1 = Expression.Variable(parameters[0].ParameterType);
        var param1 = Expression.Parameter(typeof(A1));
        var arg2 = Expression.Variable(parameters[1].ParameterType);
        var param2 = Expression.Parameter(typeof(A2));
        return Expression.Lambda<Func<T, A1, A2, R>>(
                Expression.Block(
                        typeof(R),
                        [arg1, arg2],
                        ExpressionConvertAssign(arg1, param1, parameters[0].ParameterType, typeof(A1)),
                        ExpressionConvertAssign(arg2, param2, parameters[1].ParameterType, typeof(A2)),
                        ExpressionConvert(Expression.Call(receiver, method, arg1, arg2), typeof(R), method.ReturnParameter.ParameterType)
                    ),
                receiver,
                param1,
                param2
            ).Compile();
    }

    public static Action<T> GetAction<T>(MethodInfo method)
    {
        var receiver = Expression.Parameter(typeof(T));
        return Expression.Lambda<Action<T>>(
                Expression.Call(receiver, method),
                receiver
            ).Compile();
    }

    public static Action<T, A> GetAction<T, A>(MethodInfo method)
    {
        var receiver = Expression.Parameter(typeof(T));
        var parameters = method.GetParameters();
        var arg1 = Expression.Variable(parameters[0].ParameterType);
        var param1 = Expression.Parameter(typeof(A));
        return Expression.Lambda<Action<T, A>>(
                Expression.Block(
                        [arg1],
                        ExpressionConvertAssign(arg1, param1, parameters[0].ParameterType, typeof(A)),
                        Expression.Call(receiver, method, arg1)
                    ),
                receiver,
                param1
            ).Compile();
    }

    public static Action<T, A1, A2> GetAction<T, A1, A2>(MethodInfo method)
    {
        var receiver = Expression.Parameter(typeof(T));
        var parameters = method.GetParameters();
        var arg1 = Expression.Variable(parameters[0].ParameterType);
        var param1 = Expression.Parameter(typeof(A1));
        var arg2 = Expression.Variable(parameters[1].ParameterType);
        var param2 = Expression.Parameter(typeof(A2));
        return Expression.Lambda<Action<T, A1, A2>>(
                Expression.Block(
                        [arg1, arg2],
                        ExpressionConvertAssign(arg1, param1, parameters[0].ParameterType, typeof(A1)),
                        ExpressionConvertAssign(arg2, param2, parameters[1].ParameterType, typeof(A2)),
                        Expression.Call(receiver, method, arg1, arg2)
                    ),
                receiver,
                param1,
                param2
            ).Compile();
    }

    public static Func<T> GetNew<T>()
    {
        return Expression.Lambda<Func<T>>(
                Expression.New(typeof(T).GetConstructor([])!)
            ).Compile();
    }

    public static Expression ExpressionConvertAssign(ParameterExpression left, Expression right, Type left_type, Type right_type) => Expression.Assign(left, ExpressionConvert(right, left_type, right_type));

    public static Expression ExpressionConvert(Expression right, Type left_type, Type right_type)
    {
        if (left_type == right_type) return right;
        if (left_type == typeof(string)) return Expression.Call(right, right_type.GetMethod("ToString", [])!);
        return Expression.Convert(right, left_type);
    }
}
