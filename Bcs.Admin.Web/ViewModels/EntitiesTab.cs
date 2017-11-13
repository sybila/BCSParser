using System.Collections.Generic;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Controls;
using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using BcsAdmin.BL.Filters;
using Riganti.Utils.Infrastructure;

namespace Bcs.Admin.Web.ViewModels
{
    public class EntitiesTab : DotvvmViewModelBase
    {
        private readonly DashboardFacade dashboardFacade;
        private readonly BasicListFacade basicListFacade;

        public string Name { get; set; }

        public string SearchText { get; set; }

        public GridViewDataSet<BiochemicalEntityRowDto> EntityDataSet { get; set; }

        public IList<string> EntityTypes { get; set; } = new List<string>();

        public List<string> SelectedTypes { get; set; } = new List<string>();

        public EntitiesTab(DashboardFacade dashboardFacade, BasicListFacade basicListFacade)
        {
            this.dashboardFacade = dashboardFacade;
            this.basicListFacade = basicListFacade;
            Name = "Entities";
        }

        public override Task Init()
        {
            EntityDataSet = new GridViewDataSet<BiochemicalEntityRowDto>()
            {
                PagingOptions =
                {
                    PageSize = 10
                },
                SortingOptions = new SortingOptions
                {
                    SortExpression = nameof(BiochemicalEntityRowDto.Name)
                }
            };
            EntityTypes = basicListFacade.GetEntityTypes();
            return base.Init();
        }

        public override Task PreRender()
        {
            if (!Context.IsPostBack || EntityDataSet.IsRefreshRequired)
            {
                dashboardFacade.FillDataSet(EntityDataSet, new BiochemicalEntityFilter{
                    EntityTypeFilter = SelectedTypes,
                    SearchText = SearchText
                });
            }
            return base.PreRender();
        }
    }
}

