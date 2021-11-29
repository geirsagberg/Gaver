using System;
using System.Linq.Expressions;
using AutoMapper;

namespace Gaver.Common.Extensions;

public static class MapperExtensions
{
    public static IMappingExpression<TFrom, TTo> MapMember<TFrom, TTo, TFromProp, TToProp>(
        this IMappingExpression<TFrom, TTo> expression, Expression<Func<TTo, TToProp>> to,
        Expression<Func<TFrom, TFromProp>> from)
    {
        return expression.ForMember(to, o => o.MapFrom(from));
    }

    public static IMappingExpression<TFrom, TTo> IgnoreMember<TFrom, TTo>(
        this IMappingExpression<TFrom, TTo> expression, Expression<Func<TTo, object>> to)
    {
        return expression.ForMember(to, o => o.Ignore());
    }
}