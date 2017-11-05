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

        public List<EntityCategory> Categories { get; set; }
        public EntityCategory Selected { get; set; }
        public BiochemicalEntityDetail Detail { get; set; }

        public Dashboard(DashboardFacade dashboardFacade, IMapper mapper)
        {
            this.dashboardFacade = dashboardFacade;
            this.mapper = mapper;

            Title = "Dashboard";
        }

        public override Task PreRender()
        {
            if (!Context.IsPostBack) {
                Categories = new List<EntityCategory>
                {
                    new EntityCategory()
                    {
                        Name = "Entities",
                        EntityDataSet = GridViewDataSet.Create(dashboardFacade.GetBiochemicalEntityRows().GetDataFromQueryable, 20),
                        EntityTypes = dashboardFacade.GetEntityTypes(),
                        SelectedTypes = new List<string>()
                    }
                };
                Selected = Selected ?? Categories.FirstOrDefault();
            }
            return base.PreRender();
        }

        public void EditEntity(int entityId)
        {
            var dto = dashboardFacade.GetEntityDetail(entityId);
            Detail = mapper.Map<BiochemicalEntityDetail>(dto);
        }
    }

    public class EntityCategory
    {
        public string Name { get; set; }

        public string SearchText { get; set; }

        public GridViewDataSet<BiochemicalEntityRowDto> EntityDataSet { get; set; }

        public List<string> EntityTypes { get; set; }

        public List<string> SelectedTypes { get; set; }

        public EntityCategory()
        {
        }

        public void FilterCategory()
        {
        }
    }
}

