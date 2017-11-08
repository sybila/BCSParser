using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Controls;
using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using AutoMapper;

namespace Bcs.Admin.Web.ViewModels
{
    public class Masterpage : DotvvmViewModelBase
    {
        public string Title { get; set; }
        public List<string> Errors { get; set; }
    }

    public class Dashboard : Masterpage
    {
        private readonly DashboardFacade dashboardFacade;
        private readonly IMapper mapper;

        public EntitiesTab EntitiesTab { get; set; }
        public BiochemicalEntityDetail Detail { get; set; }

        public Dashboard(DashboardFacade dashboardFacade, IMapper mapper, EntitiesTab entitiesTab)
        {
            this.dashboardFacade = dashboardFacade;
            this.mapper = mapper;
            EntitiesTab = entitiesTab;

            Title = "Dashboard";
        }

        public void EditEntity(int entityId)
        {
            var dto = dashboardFacade.GetEntityDetail(entityId);
            Detail = mapper.Map<BiochemicalEntityDetail>(dto);
        }

        public override Task Init()
        {
            EntitiesTab.Init();
            return base.Init();
        }
    }

    public class EntitiesTab : DotvvmViewModelBase
    {
        private readonly DashboardFacade dashboardFacade;

        public string Name { get; set; }

        public string SearchText { get; set; }

        public GridViewDataSet<BiochemicalEntityRowDto> EntityDataSet { get; set; }

        public List<string> EntityTypes { get; set; } = new List<string>();

        public List<string> SelectedTypes { get; set; } = new List<string>();

        public EntitiesTab(DashboardFacade dashboardFacade)
        {
            this.dashboardFacade = dashboardFacade;
            Name = "Entities";
        }

        public override Task Init()
        {
            LoadGrid();
            EntityTypes = dashboardFacade.GetEntityTypes();
            return base.Init();
        }

      
        public void LoadGrid()
        {
            EntityDataSet = GridViewDataSet.Create(LoadEntities, 10);
        }

        private GridViewDataSetLoadedData<BiochemicalEntityRowDto> LoadEntities(IGridViewDataSetLoadOptions gridViewDataSetLoadOptions)
        {
            return dashboardFacade.GetBiochemicalEntityRows(gridViewDataSetLoadOptions, SearchText, SelectedTypes);
        }

    }
}

