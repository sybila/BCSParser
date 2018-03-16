using System.Collections.Generic;
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
            await DataSet.RequestRefreshAsync(true);
            if (goTofirstPage)
            {
                await DataSet.GoToFirstPageAsync();
            }
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

        public override Task Load()
        {
            DataSet.OnLoadingData = options => listFacade.GetData(options, Filter);
            return base.Load();
        }

        public override Task PreRender()
        {
            return base.PreRender();
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

        public override Task Init()
        {
            EntityTypes = basicListFacade.GetEntityTypeNames();
            return base.Init();
        }
    }
}

