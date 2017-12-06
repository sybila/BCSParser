using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using AutoMapper;
using BcsAdmin.BL.Dto;
using System.Collections.Generic;
using System;

namespace Bcs.Admin.Web.ViewModels
{
    public class Dashboard : Masterpage
    {
        private readonly BasicListFacade basicListFacade;
        private readonly BiochemicalEntityFacade dashboardFacade;
        private readonly IMapper mapper;

        public string ActiveTabName { get; set; }
        public EntitiesTab EntitiesTab { get; set; }
        public ReactionsTab ReactionsTab { get; set; }

        public bool EntitiesSelected => ActiveTabName == EntitiesTab.Name;
        public bool ReactionsSelected => ActiveTabName == ReactionsTab.Name;

        public BiochemicalEntityDetail Detail { get; set; }
        public List<BiochemicalEntityTypeDto> HierarchyTypes { get; set; }

        public List<SuggestionDto> EntitySuggestions { get; set; }



        public Dashboard(
            BasicListFacade basicListFacade,
            BiochemicalEntityFacade dashboardFacade,
            IMapper mapper, 
            EntitiesTab entitiesTab,
            ReactionsTab rulesTab,
            BiochemicalEntityDetail detail)
        {
            this.basicListFacade = basicListFacade;
            this.dashboardFacade = dashboardFacade;
            this.mapper = mapper;
            Detail = detail;
            Title = "Dashboard";
            EntitiesTab = entitiesTab;
            ReactionsTab = rulesTab;
            ActiveTabName = entitiesTab.Name;
        }

        public void EditEntity(int entityId)
        {
            var dto = dashboardFacade.GetDetail(entityId);
            mapper.Map(dto, Detail);
            Detail.PoputateGrids();
            Detail.CancelAllActions();
        }

        public async Task Refresh()
        {
            await EntitiesTab.DataSet.RequestRefreshAsync();
            await EntitiesTab.DataSet.GoToFirstPageAsync();
        }

        public override Task Init()
        {
            HierarchyTypes = basicListFacade.GetEntityTypes();
            return base.Init();
        }

        public override Task PreRender()
        {
            return base.PreRender();
        }
    }
}

