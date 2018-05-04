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
using DotVVM.Framework.ViewModel;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalEntityDetail : DetailBase<BiochemicalEntityDetailDto>, IValidatableObject
    {
        private readonly BasicListFacade basicListFacade;
        private readonly AdvancedEntitySearchFacade usageFacade;

        [Bind(Direction.None)]
        protected BiochemicalEntityFacade EntityFacade => (BiochemicalEntityFacade)Facade;


        [Required]
        [Display(GroupName = "Fields", Name = "Entity type")]
        public int SelectedHierarchyType { get; set; }

        [Display(AutoGenerateField = false)]
        public BiochemicalEntityLinkDto Parent { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableLinkGrid<ComponentLinkDto, EntitySuggestionQuery> Components { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableGrid<string, StateEntityDto> States { get; set; }

       

        [Display(GroupName = "Grids")]
        public IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> Locations { get; set; }

        public List<BiochemicalEntityTypeDto> AllowedChildHierarchyTypes { get; set; }

        public BiochemicalEntityDetail(
            BiochemicalEntityFacade dashboardFacade,
            BasicListFacade basicListFacade,
            AdvancedEntitySearchFacade usageFacade,
            IMapper mapper,
            IEditableLinkGrid<ComponentLinkDto, EntitySuggestionQuery> componentGrid,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid,
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid,
            IEditableLinkGrid<OrganismDto, OrganismSuggestionQuery> organisms,
            IEditableGrid<string, StateEntityDto> stateGrid,
            IEditableGrid<int, EntityNoteDto> noteGrid,
            IEditableGrid<int, AnnotationDto> annotationGrid)
            : base(dashboardFacade, mapper, annotationGrid, classificationGrid, organisms, noteGrid)
        {
            this.basicListFacade = basicListFacade;
            this.usageFacade = usageFacade;
            Components = componentGrid;
            States = stateGrid;
            Locations = locationGrid;

            Organisms.ParentRepositoryName = "entities";
            Classifications.ParentRepositoryName = "entities";
            Annotations.ParentRepositoryName = "entities";
            Notes.ParentRepositoryName = "entities";
        }

        public override async Task PoputateGridsAsync()
        {
            AllowedChildHierarchyTypes = basicListFacade.GetEntityTypesForParentType(SelectedHierarchyType);

            Components.ParentEntityId =
                SelectedHierarchyType > 1 && SelectedHierarchyType != 4
                ? Id
                : 0;
            await Components.Init();
            await Components.ReloadDataAsync();
            Components.EntitySearchSelect.Filter.AllowedEntityTypes =
                AllowedChildHierarchyTypes
                .Select(t => (HierarchyType)t.Id)
                .ToArray();

            States.ParentEntityId =
              SelectedHierarchyType == 4
              ? Id
              : 0;
            await States.Init();
            await States.ReloadDataAsync();

            Locations.ParentEntityId = Id;
            await Locations.Init();
            await Locations.ReloadDataAsync();
            Locations.EntitySearchSelect.Filter.AllowedEntityTypes = new[] { HierarchyType.Compartment };

            Classifications.EntitySearchSelect.Filter.Category = CategoryType.Entity;

            await base.PoputateGridsAsync();
        }

        public override void CancelAllActions()
        {
            States.Cancel();
            Components.Cancel();
            Locations.Cancel();

            base.CancelAllActions();
        }

        public async Task AskDelete()
        {
            var usages = await usageFacade.GetEntityUsageListAsync(Id);

            if (usages.Count > 0)
            {
                Alert = new AlertViewModel
                {
                    AlertHeading = "Delete entity",
                    AlertText = "This entity is used by other entities are you sure you want to delete it?",
                    AlertItems = usages
                };
            }
            else
            {
                await DeleteAsync();
            }
        }

        public IEnumerable<ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            bool isPrimitive =
                SelectedHierarchyType == (int)HierarchyType.State
                || SelectedHierarchyType == (int)HierarchyType.Compartment;

            bool hasComponents = Components.ItemsCount != 0;
            bool hasStates = States.ItemsCount != 0;

            if (isPrimitive && (hasComponents || hasStates))
            {
                return new[] { new ValidationResult($"This entity cannot be of type {(HierarchyType)SelectedHierarchyType} while it has still components or states attached.", new[] { nameof(SelectedHierarchyType) }) };
            }

            if (SelectedHierarchyType != (int)HierarchyType.Atomic && hasStates)
            {
                return new[] { new ValidationResult($"This entity cannot be of type {(HierarchyType)SelectedHierarchyType} while it still has states attached.", new[] { nameof(SelectedHierarchyType) }) };
            }

            if (SelectedHierarchyType == (int)HierarchyType.Atomic && hasComponents)
            {
                return new[] { new ValidationResult("This entity cannot be of type Atomic while it still has components attached.", new[] { nameof(SelectedHierarchyType) }) };
            }


            return new ValidationResult[] { };
        }

        public override async Task Load()
        {
            Alert = null;
            await base.Load();
        }
    }
}
