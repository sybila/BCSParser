using BcsAdmin.BL.Dto;
using DotVVM.Framework.Hosting;
using Riganti.Utils.Infrastructure;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bcs.Admin.Web.ViewModels.Grids;
using BcsAdmin.BL.Facades;
using AutoMapper;
using BcsAdmin.BL.Queries;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalEntityDetail : DetailBase
    {
        private readonly BiochemicalEntityFacade entityFacade;
       
        [Required]
        [Display(GroupName = "Fields", Name = "Entity type")]
        public int SelectedHierarchyType { get; set; }

        [Display(AutoGenerateField = false)]
        public BiochemicalEntityLinkDto Parent { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableLinkGrid<ComponentLinkDto, EntitySuggestionQuery> Components { get; set; }     

        public BiochemicalEntityDetail(
            BiochemicalEntityFacade dashboardFacade,
            IMapper mapper,
            IEditableLinkGrid<ComponentLinkDto, EntitySuggestionQuery> componentGrid,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid,
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid,
            IEditableLinkGrid<EntityOrganismDto, OrganismSuggestionQuery> organisms,
            IEditableGrid<EntityNoteDto> noteGrid)
            : base(mapper, locationGrid, classificationGrid, organisms, noteGrid)
        {
            this.entityFacade = dashboardFacade;
            Components = componentGrid;
        }

        public override void PoputateGrids()
        {
            Components.ParentEntityId = Id;
            Components.Init();

            base.PoputateGrids();
        }

        public override void CancelAllActions()
        {
            Components.Cancel();

            base.CancelAllActions();
        }

        public override void Save()
        {
            var dto = Mapper.Map<BiochemicalEntityDetailDto>(this);
            entityFacade.Save(dto);
        }
    }
}
