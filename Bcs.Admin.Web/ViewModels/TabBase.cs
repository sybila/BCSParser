﻿using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Controls;
using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using BcsAdmin.BL;

namespace Bcs.Admin.Web.ViewModels
{
    public class TabBase<TGridDto, TFilter> : AppViewModelBase
    {
        private readonly IQueryFacade<TGridDto, TFilter> listFacade;

        public GridViewDataSet<TGridDto> DataSet { get; set; }
        public TFilter Filter { get; set; }

        public string Name { get; set; }

        public TabBase(IQueryFacade<TGridDto, TFilter> listFacade)
        {
            this.listFacade = listFacade;
        }

        public async Task RefreshAsync(bool goTofirstPage = false)
        {
            await ExecuteSafeAsync(async () =>
            {
                if (goTofirstPage)
                {
                    DataSet.GoToFirstPage();
                }
                await ReloadDataAsync();
            });           
        }

        private async Task ReloadDataAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                await listFacade.FillDataSetAsync(DataSet, Filter);
                DataSet.IsRefreshRequired = false;
            });
        }

        public override Task Init()
        {
            if (!Context.IsPostBack)
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
            }
            return base.Init();
        }

        public override async Task PreRender()
        {
            await ExecuteSafeAsync(async () =>
            {
                if (!Context.IsPostBack || DataSet?.IsRefreshRequired == true)
                {
                    await ReloadDataAsync();
                }
                await base.PreRender();
            });
        }
    }
}

