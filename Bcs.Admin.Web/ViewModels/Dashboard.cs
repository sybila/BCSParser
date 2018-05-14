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

        public NewEntityWizard NewEntityWizard { get; set; }
        public BiochemicalEntityDetail EntityDetail { get; set; }
        public BiochemicalReactionDetail ReactionDetail { get; set; }


        public List<BiochemicalEntityTypeDto> HierarchyTypes { get; set; }
        public List<StatusDto> BcsObjectStatuses { get; set; }
        public List<ClassificationTypeDto> ClassificationTypes { get; set; }
        public List<AnnotationTypeDto> AnnotationTypes { get; set; }
        public List<int> PaginationOptions { get; set; } = new List<int> { 10, 30, 50, 100, 200 };

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
            NewEntityWizard newEntityWizard)
        {
            this.basicListFacade = basicListFacade;
            this.dashboardFacade = dashboardFacade;
            this.reactionFacade = reactionFacade;
            this.mapper = mapper;

            EntityDetail = entityDetail;
            ReactionDetail = reactionDetail;
            NewEntityWizard = newEntityWizard;
            NewEntityWizard.SetParent(this);
            Title = "Dashboard";
            EntitiesTab = entitiesTab;
            ReactionsTab = rulesTab;
            ActiveTabName = entitiesTab.Name;

            EntityDetail.UpdateGrid = async () => await EntitiesTab.RefreshAsync();
            ReactionDetail.UpdateGrid = async () => await ReactionsTab.RefreshAsync();
        }

        public void NewEntity()
        {
            NewEntityWizard.StartNew();
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
                AnnotationTypes = basicListFacade.GetAnnotationTypes();
                BcsObjectStatuses = basicListFacade.GetBcsObjectStatuses();
                ClassificationTypes = basicListFacade.GetClassificationTypes();
            }
            return base.Load();
        }
    }
}

