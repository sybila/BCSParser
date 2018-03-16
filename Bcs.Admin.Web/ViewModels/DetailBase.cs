using BcsAdmin.BL.Dto;
using DotVVM.Framework.ViewModel;
using System.ComponentModel.DataAnnotations;
using Bcs.Admin.Web.ViewModels.Grids;
using AutoMapper;
using BcsAdmin.BL.Queries;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace Bcs.Admin.Web.ViewModels
{
    public abstract class DetailBase<TDto> : DotvvmViewModelBase, IEntity<int>
        where TDto : IEntity<int>
    {
        [Bind(Direction.None)]
        protected IMapper Mapper { get; }

        [Bind(Direction.None)]
        protected ICrudDetailFacade<TDto, int> Facade { get; }

        [Protect(ProtectMode.SignData)]
        [Display(AutoGenerateField = false)]
        public int Id { get; set; } = -1;

        //[Required]
        [Display(GroupName = "Fields", Name = "Name")]
        public string Name { get; set; }

        //[Required]
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

        public virtual void PoputateGrids()
        {
            Classifications.ParentEntityId = Id;
            Classifications.Init();
            Classifications.DataSet.RequestRefresh(true);

            Organisms.ParentEntityId = Id;
            Organisms.Init();
            Organisms.DataSet.RequestRefresh(true);

            Locations.ParentEntityId = Id;
            Locations.Init();
            Locations.DataSet.RequestRefresh(true);

            Notes.ParentEntityId = Id;
            Notes.Init();
            Notes.DataSet.RequestRefresh(true);
        }

        public virtual void CancelAllActions()
        {
            Classifications.Cancel();
            Locations.Cancel();
            Organisms.Cancel();
            Notes.Cancel();
        }

        public void Save()
        {
            var dto = Mapper.Map<TDto>(this);
            Facade.Save(dto);
        }

        public void Delete()
        {
            Facade.Delete(Id);
            Close();
        }

        public void Close()
        {
            var @new = Facade.InitializeNew();
            Mapper.Map(@new, this);
            Id = -1;
        }
    }
}
