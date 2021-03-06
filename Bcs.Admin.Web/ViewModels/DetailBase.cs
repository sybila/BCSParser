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
    public abstract class DetailBase<TDto> : AppViewModelBase, IEntity<int>
        where TDto : IEntity<int>
    {
        [Bind(Direction.None)]
        protected IMapper Mapper { get; }

        [Bind(Direction.None)]
        protected IAsyncDetailFacade<TDto, int> Facade { get; }

        [Bind(Direction.None)]
        public Func<Task> UpdateGrid { get; set; }

        [Protect(ProtectMode.SignData)]
        [Display(AutoGenerateField = false)]
        public int Id { get; set; } = -1;

        [Display(GroupName = "Fields", Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(GroupName = "Fields", Name = "State")]
        public int Status { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableGrid<int, AnnotationDto> Annotations { get; set; }

        [Display(GroupName = "Grids", Name = "Classification")]
        public IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> Classifications { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableLinkGrid<OrganismDto, OrganismSuggestionQuery> Organisms { get; set; }

        [Display(GroupName = "Grids")]
        public IEditableGrid<int, EntityNoteDto> Notes { get; set; }

        public AlertViewModel Alert { get; set; }

        public DetailBase(
            IAsyncDetailFacade<TDto, int> facade,
            IMapper mapper,
            IEditableGrid<int, AnnotationDto> annotationGrid,
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid,
            IEditableLinkGrid<OrganismDto, OrganismSuggestionQuery> organisms,
            IEditableGrid<int, EntityNoteDto> noteGrid)
        {
            Mapper = mapper;
            Facade = facade;

            Annotations = annotationGrid;
            Classifications = classificationGrid;
            Notes = noteGrid;
            Organisms = organisms;
        }

        public virtual async Task PoputateGridsAsync()
        {
            Annotations.ParentEntityId = Id;
            await Annotations.Init();
            await Annotations.ReloadDataAsync();

            Classifications.ParentEntityId = Id;
            await Classifications.Init();
            await Classifications.ReloadDataAsync();

            Organisms.ParentEntityId = Id;
            await Organisms.Init();
            await Organisms.ReloadDataAsync();

            Notes.ParentEntityId = Id;
            await Notes.Init();
            await Notes.ReloadDataAsync();
        }

        public virtual void CancelAllActions()
        {
            Annotations.Cancel();
            Classifications.Cancel();
            Organisms.Cancel();
            Notes.Cancel();
        }

        public async Task SaveAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                var dto = Mapper.Map<TDto>(this);
                AfterMap(dto);

                await Facade.SaveAsync(dto);
                Mapper.Map(dto, this);

                await PoputateGridsAsync();
                CancelAllActions();
            }, "Changes saved.");
        }

        protected virtual void AfterMap(TDto dto)
        {
        }

        public virtual async Task DeleteAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                await Facade.DeleteAsync(Id);
                Close();
                await UpdateGrid?.Invoke();
            }, "Entity deleted");
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
            var dto = await Facade.GetDetailAsync(id);
            Mapper.Map(dto, this);
            await PoputateGridsAsync();
            CancelAllActions();
        }
    }
}
