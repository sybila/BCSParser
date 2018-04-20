﻿using System.Collections.Generic;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Controls;
using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using BcsAdmin.BL.Filters;
using Riganti.Utils.Infrastructure;
using Riganti.Utils.Infrastructure.Services.Facades;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL;
using BcsAdmin.BL.Dto;
using Bcs.Admin.BL.Dto;

namespace Bcs.Admin.Web.ViewModels
{
    public class TabBase<TGridDto, TFilter> : DotvvmViewModelBase
    {
        private readonly IListFacade<TGridDto, TFilter> listFacade;

        public GridViewDataSet<TGridDto> DataSet { get; set; }
        public TFilter Filter { get; set; }

        public string Name { get; set; }

        public TabBase(IListFacade<TGridDto, TFilter> listFacade)
        {
            this.listFacade = listFacade;
        }

        public async Task RefreshAsync(bool goTofirstPage = false)
        {
            if (goTofirstPage)
            {
                DataSet.GoToFirstPage();
            }
            await ReloadDataAsync();
        }

        private async Task ReloadDataAsync()
        {
            await listFacade.FillDataSetAsync(DataSet, Filter);
            DataSet.IsRefreshRequired = false;
        }

        public override Task Init()
        {
            DataSet = new GridViewDataSet<TGridDto>()
            {
                PagingOptions =
                {
                    PageSize = 10
                },
                SortingOptions = new SortingOptions
                {
                    SortExpression = "Name"
                }
            };
            return base.Init();
        }

        public async override Task Load()
        {
            //if (!Context.IsPostBack)
            //{
                await ReloadDataAsync();
            //}
            await base.Load();
        }
    }

    public class ReactionsTab : TabBase<ReactionRowDto, ReactionFilter>
    {
        public ReactionsTab(ReactionFacade reactionFacade)
            : base(reactionFacade)
        {
            Name = "Reactions";
            Filter = new ReactionFilter();
        }
    }

    public class EntitiesTab : TabBase<BiochemicalEntityRowDto, BiochemicalEntityFilter>
    {
        private readonly BasicListFacade basicListFacade;

        public IList<string> EntityTypes { get; set; } = new List<string>();

        public EntitiesTab(BiochemicalEntityFacade dashboardFacade, BasicListFacade basicListFacade)
            : base(dashboardFacade)
        {
            this.basicListFacade = basicListFacade;
            Name = "Entities";
            Filter = new BiochemicalEntityFilter();
        }

        public async override Task Init()
        {
            EntityTypes = await basicListFacade.GetEntityTypeNames();
            await base.Init();
        }
    }
}

