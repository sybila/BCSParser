using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using BcsAdmin.BL.Facades;
using Riganti.Utils.Infrastructure;

namespace BcsAdmin.BL
{
    public static class Extensions
    {
        public static void FillDataSet<TListDTO, TFilterDTO>(this IListFacade<TListDTO, TFilterDTO> facade, GridViewDataSet<TListDTO> dataSet, TFilterDTO filter)
        {
            using (facade.UnitOfWorkProvider.Create())
            {
                var query = facade.QueryFactory();
                query.Filter = filter;
                dataSet.LoadFromQuery(query);
            }
        }

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
