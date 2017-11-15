using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using AutoMapper;
using BcsAdmin.BL.Dto;
using System.Collections.Generic;

namespace Bcs.Admin.Web.ViewModels
{
    public class Dashboard : Masterpage
    {
        private readonly BasicListFacade basicListFacade;
        private readonly DashboardFacade dashboardFacade;
        private readonly IMapper mapper;

        public EntitiesTab EntitiesTab { get; set; }
        public BiochemicalEntityDetail Detail { get; set; }
        public List<BiochemicalEntityTypeDto> HierarchyTypes { get; set; }

        public Dashboard(
            BasicListFacade basicListFacade,
            DashboardFacade dashboardFacade,
            IMapper mapper, 
            EntitiesTab entitiesTab,
            BiochemicalEntityDetail biochemicalEntityDetail)
        {
            this.basicListFacade = basicListFacade;
            this.dashboardFacade = dashboardFacade;
            this.mapper = mapper;
            EntitiesTab = entitiesTab;
            Detail = biochemicalEntityDetail;
            Title = "Dashboard";
        }

        public void EditEntity(int entityId)
        {
            var dto = dashboardFacade.GetDetail(entityId);
            mapper.Map(dto, Detail);
            Detail.Init();
        }

        public void Refresh()
        {
            EntitiesTab.EntityDataSet.RequestRefresh();
        }

        public override Task Init()
        {
            HierarchyTypes = basicListFacade.GetEntityTypes();;
            return base.Init();
        }

        public override Task PreRender()
        {
            return base.PreRender();
        }
    }
}

