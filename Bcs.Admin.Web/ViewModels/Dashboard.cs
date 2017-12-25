using BcsAdmin.BL.Facades;
using System.Threading.Tasks;
using AutoMapper;
using BcsAdmin.BL.Dto;
using System.Collections.Generic;
using System;
using Bcs.Admin.BL.Dto;

namespace Bcs.Admin.Web.ViewModels
{
    public class Dashboard : Masterpage
    {
        private readonly BasicListFacade basicListFacade;
        private readonly BiochemicalEntityFacade dashboardFacade;
        private readonly ReactionFacade reactionFacade;
        private readonly IMapper mapper;

        public string ActiveTabName { get; set; }
        public EntitiesTab EntitiesTab { get; set; }
        public ReactionsTab ReactionsTab { get; set; }

        public List<DetailBase> OpenDetails { get; set; }

        public bool EntitiesSelected => ActiveTabName == EntitiesTab.Name;
        public bool ReactionsSelected => ActiveTabName == ReactionsTab.Name;

        public BiochemicalEntityDetail EntityDetail { get; set; }
        public BiochemicalReactionDetail ReactionDetail { get; set; }
        public List<BiochemicalEntityTypeDto> HierarchyTypes { get; set; }

        public List<SuggestionDto> EntitySuggestions { get; set; }



        public Dashboard(
            BasicListFacade basicListFacade,
            BiochemicalEntityFacade dashboardFacade,
            ReactionFacade reactionFacade, 
            IMapper mapper, 
            EntitiesTab entitiesTab,
            ReactionsTab rulesTab,
            BiochemicalEntityDetail entityDetail,
            BiochemicalReactionDetail reactionDetail)
        {
            this.basicListFacade = basicListFacade;
            this.dashboardFacade = dashboardFacade;
            this.reactionFacade = reactionFacade;
            this.mapper = mapper;
            EntityDetail = entityDetail;
            ReactionDetail = reactionDetail;
            Title = "Dashboard";
            EntitiesTab = entitiesTab;
            ReactionsTab = rulesTab;
            ActiveTabName = entitiesTab.Name;
        }

        public void EditEntity(int entityId)
        {
            var dto = dashboardFacade.GetDetail(entityId);
            mapper.Map(dto, EntityDetail);
            EntityDetail.PoputateGrids();
            EntityDetail.CancelAllActions();

            //OpenDetails.Add(EntityDetail);
        }

        public void EditReaction(int reactionId)
        {
            var dto = reactionFacade.GetDetail(reactionId);
            mapper.Map(dto, ReactionDetail);
            ReactionDetail.PoputateGrids();
            ReactionDetail.CancelAllActions();

            //OpenDetails.Add(ReactionDetail);
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
    }
}

