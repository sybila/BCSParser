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
using System.Threading.Tasks;

namespace BcsAdmin.BL
{
    public static class Extensions
    {
        public async static Task LoadFromQueryAsync<T>(this GridViewDataSet<T> dataSet, IQuery<T> query)
        {
            query.Skip = dataSet.PagingOptions.PageIndex * dataSet.PagingOptions.PageSize;
            query.Take = dataSet.PagingOptions.PageSize;
            query.ClearSortCriteria();

            if (!string.IsNullOrEmpty(dataSet.SortingOptions.SortExpression))
            {
                query.AddSortCriteria(dataSet.SortingOptions.SortExpression, dataSet.SortingOptions.SortDescending ? SortDirection.Descending : SortDirection.Ascending);
            }

            dataSet.PagingOptions.TotalItemsCount = await query.GetTotalRowCountAsync();
            dataSet.Items = await query.ExecuteAsync();
        }

        public async static Task FillDataSetAsync<TListDTO, TFilterDTO>(this IQueryFacade<TListDTO, TFilterDTO> facade, GridViewDataSet<TListDTO> dataSet, TFilterDTO filter)
        {
            var query = facade.QueryFactory();
            query.Filter = filter;
            await dataSet.LoadFromQueryAsync(query);
        }
    }
}
