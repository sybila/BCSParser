using System.Collections.Generic;
using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using BcsAdmin.BL.Filters;
using Riganti.Utils.Infrastructure;
using Riganti.Utils.Infrastructure.Services.Facades;
using Riganti.Utils.Infrastructure.Core;
using BcsAdmin.BL;
using Bcs.Admin.BL.Dto;

namespace Bcs.Admin.Web.ViewModels
{
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

