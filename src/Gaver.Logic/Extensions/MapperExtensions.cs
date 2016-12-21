using System;
using System.Linq.Expressions;
using AutoMapper;

namespace Gaver.Logic
{
    public static class MapperExtensions
    {
        public static IMappingExpression<TFrom, TTo> MapMember<TFrom, TTo>(this IMappingExpression<TFrom, TTo> expression, Expression<Func<TTo, object>> to, Expression<Func<TFrom, object>> from)
        {
            return expression.ForMember(to, o => o.MapFrom(from));
        }

        public static IMappingExpression<TFrom, TTo> IgnoreMember<TFrom, TTo>(this IMappingExpression<TFrom, TTo> expression, Expression<Func<TTo, object>> to)
        {
            return expression.ForMember(to, o => o.Ignore());
        }
    }
}