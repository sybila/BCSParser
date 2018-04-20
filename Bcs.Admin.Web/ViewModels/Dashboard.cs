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

        public bool EntitiesSelected => ActiveTabName == EntitiesTab.Name;
        public bool ReactionsSelected => ActiveTabName == ReactionsTab.Name;

        public BiochemicalEntityDetail EntityDetail { get; set; }
        public BiochemicalReactionDetail ReactionDetail { get; set; }
        public SimilarEntitySearchPanel EntitySearchPanel { get; set; }
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
            BiochemicalReactionDetail reactionDetail,
            SimilarEntitySearchPanel entitySearchPanel)
        {
            this.basicListFacade = basicListFacade;
            this.dashboardFacade = dashboardFacade;
            this.reactionFacade = reactionFacade;
            this.mapper = mapper;

            EntityDetail = entityDetail;
            ReactionDetail = reactionDetail;
            EntitySearchPanel = entitySearchPanel;
            Title = "Dashboard";
            EntitiesTab = entitiesTab;
            ReactionsTab = rulesTab;
            ActiveTabName = entitiesTab.Name;

            EntityDetail.UpdateGrid = () => EntitiesTab.RefreshAsync();
            ReactionDetail.UpdateGrid = () => ReactionsTab.RefreshAsync();
        }

        public async Task EditEntityAsync(int entityId)
        {
            await EntityDetail.EditAsync(entityId);
        }

        public async Task EditReactionAsync(int reactionId)
        {
            await ReactionDetail.EditAsync(reactionId);
        }

        public override Task Init()
        {
            return base.Init();
        }

        public override Task Load()
        {
            if((HierarchyTypes?.Count ?? 0) == 0)
            {
                HierarchyTypes = basicListFacade.GetEntityTypes();
            }
            return base.Load();
        }
    }
}

