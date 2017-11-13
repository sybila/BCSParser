using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using AutoMapper;

namespace Bcs.Admin.Web.ViewModels
{
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
            var dto = dashboardFacade.GetDetail(entityId);
            Detail = mapper.Map<BiochemicalEntityDetail>(dto);
        }

        public override Task Init()
        {
            EntitiesTab.Init();
            return base.Init();
        }

        public override Task PreRender()
        {
            EntitiesTab.PreRender();
            return base.PreRender();
        }
    }
}

