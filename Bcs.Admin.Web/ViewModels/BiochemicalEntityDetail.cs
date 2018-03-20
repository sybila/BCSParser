﻿using BcsAdmin.BL.Dto;
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

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalEntityDetail : DetailBase<BiochemicalEntityDetailDto>, IValidatableObject
    {
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
        public IEditableGrid<StateEntityDto> States { get; set; }

        public BiochemicalEntityDetail(
            BiochemicalEntityFacade dashboardFacade,
            IMapper mapper,
            IEditableLinkGrid<ComponentLinkDto, EntitySuggestionQuery> componentGrid,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid,
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid,
            IEditableLinkGrid<EntityOrganismDto, OrganismSuggestionQuery> organisms,
            IEditableGrid<StateEntityDto> stateGrid,
            IEditableGrid<EntityNoteDto> noteGrid)
            : base(dashboardFacade, mapper, locationGrid, classificationGrid, organisms, noteGrid)
        {
            Components = componentGrid;
            States = stateGrid;
        }

        public override async Task PoputateGridsAsync()
        {
            Components.ParentEntityId =
                SelectedHierarchyType > 1 && SelectedHierarchyType != 4
                ? Id
                : 0;
            await Components.Init();
            await Components.DataSet.RequestRefreshAsync(true);

            States.ParentEntityId =
              SelectedHierarchyType == 4
              ? Id
              : 0;
            await States.Init();
            await States.DataSet.RequestRefreshAsync(true);

            await base.PoputateGridsAsync();
        }

        public override void CancelAllActions()
        {
            Components.Cancel();

            base.CancelAllActions();
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
                return new[] { new ValidationResult($"This entity cannot be of type {(HierarchyType)SelectedHierarchyType} while it has still components or states attached.", new[] { nameof(SelectedHierarchyType)}) };
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
    }
}
