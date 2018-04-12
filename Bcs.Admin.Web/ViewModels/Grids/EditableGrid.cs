﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BcsAdmin.BL;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure;
using Riganti.Utils.Infrastructure.Core;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public class EditableGrid<TGridEntity> : DotvvmViewModelBase, IEditableGrid<TGridEntity>
        where TGridEntity : class, IEntity<int>, IAssociatedEntity
    {
        private readonly IGridFacade<TGridEntity> facade;

        public int ParentEntityId { get; set; }

        public GridViewDataSet<TGridEntity> DataSet { get; set; }

        public TGridEntity NewRow { get; set; }

        [Bind(Direction.ServerToClient)]
        public int ItemsCount => DataSet.PagingOptions.TotalItemsCount;

        public bool IsCollapsed { get; set; }

        public EditableGrid(IGridFacade<TGridEntity> facade)
        {
            this.facade = facade;
        }

        public void Add()
        {
            NewRow = facade.InitializeNew();
            NewRow.IntermediateEntityId = ParentEntityId;
        }

        public void Cancel()
        {
            NewRow = null;
            DataSet.RowEditOptions.EditRowId = -1;
        }

        public void Delete(TGridEntity entity)
        {
            facade.Delete(entity.Id);
            ReloadData();
        }

        public void Edit(TGridEntity entity)
        {
            Cancel();
            entity.IntermediateEntityId = ParentEntityId;
            DataSet.RowEditOptions.EditRowId = entity.Id;
        }

        public void SaveNew()
        {
            facade.Save(NewRow);
            Cancel();
            ReloadData();
        }

        public void SaveEdit(TGridEntity entity)
        {
            entity.IntermediateEntityId = ParentEntityId;
            facade.Save(entity);
            Cancel();
            ReloadData();
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

        public void ReloadData()
        {
            facade.FillDataSet(DataSet, new IdFilter { Id = ParentEntityId });
        }
    }
}
