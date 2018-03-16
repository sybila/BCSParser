using BcsAdmin.BL.Dto;
using DotVVM.Framework.ViewModel;
using System.ComponentModel.DataAnnotations;
using Bcs.Admin.Web.ViewModels.Grids;
using AutoMapper;
using BcsAdmin.BL.Queries;
using Riganti.Utils.Infrastructure.Core;

namespace Bcs.Admin.Web.ViewModels
{
    public abstract class DetailBase : DotvvmViewModelBase, IEntity<int>
    {
        [Bind(Direction.None)]
        protected IMapper Mapper { get; }

        [Protect(ProtectMode.SignData)]
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

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

        public DetailBase(IMapper mapper,
            IEditableLinkGrid<LocationLinkDto, EntitySuggestionQuery> locationGrid,
            IEditableLinkGrid<ClassificationDto, ClassificationSuggestionQuery> classificationGrid,
            IEditableLinkGrid<EntityOrganismDto, OrganismSuggestionQuery> organisms,
            IEditableGrid<EntityNoteDto> noteGrid)
        {
            Mapper = mapper;
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

        public abstract void Save();
    }
}
