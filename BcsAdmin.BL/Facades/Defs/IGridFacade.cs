using System;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Facades.Defs
{
    public interface IGridFacade<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        Func<IFilteredQuery<TEntity, IdFilter>> QueryFactory { get; }

        Task DeleteAsync(int parentId, string parentRepositoryName, TKey key);
        Task<TEntity> GetDetailAsync(int parentId, string parentRepositoryName, TKey key);
        TEntity InitializeNew();
        Task<TEntity> SaveAsync(int parentId, string parentRepositoryName, TEntity data);

        Task FillDataSetAsync(GridViewDataSet<TEntity> dataSet, IdFilter filter);
    }
}