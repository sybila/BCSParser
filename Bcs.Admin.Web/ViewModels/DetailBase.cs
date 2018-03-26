﻿using BcsAdmin.BL.Dto;
using DotVVM.Framework.ViewModel;
using System.ComponentModel.DataAnnotations;
using Bcs.Admin.Web.ViewModels.Grids;
using AutoMapper;
using BcsAdmin.BL.Queries;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Threading.Tasks;
using BcsAdmin.BL.Facades;

namespace Bcs.Admin.Web.ViewModels
{
    public abstract class DetailBase<TDto> : DotvvmViewModelBase, IEntity<int>
        where TDto : IEntity<int>
    {
        [Bind(Direction.None)]
        protected IMapper Mapper { get; }

        [Bind(Direction.None)]
        protected ICrudDetailFacade<TDto, int> Facade { get; }

        [Bind(Direction.None)]
        public Func<Task> UpdateGrid { get; set; }

        [Protect(ProtectMode.SignData)]
        [Display(AutoGenerateField = false)]
        public int Id { get; set; } = -1;

        [Display(GroupName = "Fields", Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(GroupName = "Fields", Name = "Code")]
        public string Code { get; set; }

        [Display(GroupName = "Fields", Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(GroupName = "Fields", Name = "Xml for visualisation")]
        [DataType(DataType.MultilineText)]
        public string VisualisationXml { get; set; }

        [Display(GroupName = "Fields", Name = "Active")]
        public bool Active { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> Locations { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> Classifications { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableLinkGrid<EntityOrganismDto, OrganismSuggestionQuery> Organisms { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableGrid<EntityNoteDto> Notes { get; set; }

        public AlertViewModel Alert { get; set; }

        public DetailBase(
            ICrudDetailFacade<TDto, int> facade,
            IMapper mapper,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid,
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid,
            IEditableLinkGrid<EntityOrganismDto, OrganismSuggestionQuery> organisms,
            IEditableGrid<EntityNoteDto> noteGrid)
        {
            Mapper = mapper;
            Facade = facade;

            Locations = locationGrid;
            Classifications = classificationGrid;
            Notes = noteGrid;

            Organisms = organisms;
        }

        public virtual async Task PoputateGridsAsync()
        {
            Classifications.ParentEntityId = Id;
            await Classifications.Init();
            await Classifications.DataSet.RequestRefreshAsync(true);

            Organisms.ParentEntityId = Id;
            await Organisms.Init();
            await Organisms.DataSet.RequestRefreshAsync(true);

            Locations.ParentEntityId = Id;
            await Locations.Init();
            await Locations.DataSet.RequestRefreshAsync(true);
            Locations.EntitySearchSelect.Filter.AllowedEntityTypes = new[] { HierarchyType.Compartment };

            Notes.ParentEntityId = Id;
            await Notes.Init();
            await Notes.DataSet.RequestRefreshAsync(true);
        }

        public virtual void CancelAllActions()
        {
            Classifications.Cancel();
            Locations.Cancel();
            Organisms.Cancel();
            Notes.Cancel();
        }

        public async Task SaveAsync()
        {
            var dto = Mapper.Map<TDto>(this);
            Facade.Save(dto);
            Mapper.Map(dto, this);

            await PoputateGridsAsync();
            CancelAllActions();
        }

        public virtual async Task DeleteAsync()
        {
            Facade.Delete(Id);
            Close();
            await UpdateGrid?.Invoke();
        }

        public void Close()
        {
            var @new = Facade.InitializeNew();
            Mapper.Map(@new, this);
            Id = -1;
            CancelAllActions();
        }

        public async Task NewAsync()
        {
            var @new = Facade.InitializeNew();
            Mapper.Map(@new, this);
            CancelAllActions();
            await PoputateGridsAsync();
        }

        public async Task EditAsync(int id)
        {
            var dto = Facade.GetDetail(id);
            Mapper.Map(dto, this);
            await PoputateGridsAsync();
            CancelAllActions();
        }
    }
}
