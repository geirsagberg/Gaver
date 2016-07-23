using System;
using AutoMapper;

namespace Gaver.Logic
{
    public interface IMapping
    {
    }

    public interface IMapping<TFrom, TTo> : IMapping
    {
        void ConfigureMapping(IMappingExpression<TFrom, TTo> expression);
    }
}