using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BcsAdmin.BL;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Facades.Defs;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure;
using Riganti.Utils.Infrastructure.Core;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public class EditableGrid<TKey, TGridEntity> : DotvvmViewModelBase, IEditableGrid<TKey, TGridEntity>
        where TGridEntity : class, IEntity<TKey>
    {
        private readonly IGridFacade<TKey, TGridEntity> facade;

        public int ParentEntityId { get; set; }
        public string ParentRepositoryName { get; set; }

        public GridViewDataSet<TGridEntity> DataSet { get; set; }

        public TGridEntity NewRow { get; set; }

        [Bind(Direction.ServerToClient)]
        public int ItemsCount => DataSet.PagingOptions.TotalItemsCount;

        public bool IsCollapsed { get; set; }

        public EditableGrid(IGridFacade<TKey, TGridEntity> facade)
        {
            this.facade = facade;
        }

        public void Add()
        {
            NewRow = facade.InitializeNew();
        }

        public void Cancel()
        {
            NewRow = null;
            DataSet.RowEditOptions.EditRowId = -1;
        }

        public async Task DeleteAsync(TGridEntity entity)
        {
            facade.Delete(ParentEntityId, ParentRepositoryName, entity.Id);
            await ReloadDataAsync();
        }

        public void Edit(TGridEntity entity)
        {
            Cancel();
            DataSet.RowEditOptions.EditRowId = entity.Id;
        }

        public async Task SaveNewAsync()
        {
            facade.Save(ParentEntityId, ParentRepositoryName, NewRow);
            Cancel();
            await ReloadDataAsync();
        }

        public async Task SaveEditAsync(TGridEntity entity)
        {
            facade.Save(ParentEntityId, ParentRepositoryName, entity);
            Cancel();
            await ReloadDataAsync();
        }

        public override Task Init()
        {
            if (!Context.IsPostBack)
            {
                DataSet = new GridViewDataSet<TGridEntity>()
                {
                    PagingOptions = { PageSize = 10 },
                    SortingOptions = new SortingOptions { },
                    RowEditOptions = new RowEditOptions
                    {
                        PrimaryKeyPropertyName = "Id"
                    }
                };
            }
            return base.Init();
        }

        public override Task Load()
        {
            return base.Load();
        }

        public override Task PreRender()
        {
            return base.PreRender();
        }

        public async Task ReloadDataAsync()
        {
            await facade.FillDataSetAsync(DataSet, new IdFilter { Id = ParentEntityId });
        }
    }
}
