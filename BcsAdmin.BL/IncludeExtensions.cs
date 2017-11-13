using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BcsAdmin.BL
{
    public static class IncludeExtensions
    {

        public static NextExpressionIncludeCollectionDefinition<TEntity, TResultList, TResult, TNextResult> Then<TEntity, TResultList, TResult, TNextResult>(
            this IChainableIncludeDefinition<TEntity, TResultList> target,
            Expression<Func<TResult, TNextResult>> expression
        )
             where TEntity : class
             where TResultList : class, IEnumerable<TResult>
             where TResult : class
             where TNextResult : class
        {
            return new NextExpressionIncludeCollectionDefinition<TEntity, TResultList, TResult, TNextResult>(target, expression);
        }

    }
}
