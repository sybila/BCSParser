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
using BcsAdmin.BL.Queries;
using BcsAdmin.BL.Filters;

namespace BcsAdmin.BL
{
    public static class Extensions
    {
        public static GridViewDataSetLoadedData<TListDTO> GetData<TListDTO, TFilterDTO>(this IListFacade<TListDTO, TFilterDTO> facade, IGridViewDataSetLoadOptions loadOptions, TFilterDTO filter)
        {
            using (facade.UnitOfWorkProvider.Create())
            {
                var query = facade.QueryFactory();
                query.Filter = filter;
                return (GridViewDataSetLoadedData<TListDTO>)query.CastTo<AppQuery<TListDTO>>().Execute(loadOptions);
            }
        }

        public static GridViewDataSetLoadedData<TGridEntity> GetData<TGridEntity>(this IGridFacade<TGridEntity> facade, IGridViewDataSetLoadOptions loadOptions, IdFilter filter)
            where TGridEntity : IEntity<int>
        {
            using (facade.UnitOfWorkProvider.Create())
            {
                var query = facade.QueryFactory();
                query.Filter = filter;
                return (GridViewDataSetLoadedData<TGridEntity>)query.CastTo<AppQuery<TGridEntity>>().Execute(loadOptions);
            }
        }

        public static GridViewDataSetLoadedData<TGridEntity> GetData<TGridEntity>(this ILinkGridFacade<TGridEntity> facade, IGridViewDataSetLoadOptions loadOptions, IdFilter filter)
           where TGridEntity : IEntity<int>
        {
            using (facade.UnitOfWorkProvider.Create())
            {
                var query = facade.QueryFactory();
                query.Filter = filter;
                return (GridViewDataSetLoadedData<TGridEntity>)query.CastTo<AppQuery<TGridEntity>>().Execute(loadOptions);
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
