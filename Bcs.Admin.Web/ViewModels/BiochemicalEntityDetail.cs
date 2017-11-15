using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalEntityDetail : DotvvmViewModelBase
    {
        [Protect(ProtectMode.SignData)]
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Xml for visualisation")]
        [DataType(DataType.MultilineText)]
        public string VisualisationXml { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Entity type")]
        public int SelectedHierarchyType { get; set; }

        [Display(AutoGenerateField = false)]
        public BiochemicalEntityLinkDto Parent { get; set; }

        [Display(AutoGenerateField = false)]
        public EditableGrid<ComponentLinkDto> Components { get; set; }

        [Display(AutoGenerateField = false)]
        public EditableGrid<LocationLinkDto> Locations { get; set; }

        [Display(AutoGenerateField = false)]
        public EditableGrid<ClassificationDto> Classifications { get; set; }

        [Display(AutoGenerateField = false)]
        public EditableGrid<EntityNoteDto> Notes { get; set; }

        public BiochemicalEntityDetail(
          EditableGrid<ComponentLinkDto> componentGrid,
          EditableGrid<LocationLinkDto> locationGrid,
          EditableGrid<ClassificationDto> classificationGrid,
          EditableGrid<EntityNoteDto> noteGrid)
        {
            Components = componentGrid;
            Locations = locationGrid;
            Classifications = classificationGrid;
            Notes = noteGrid;
        }

        public override Task Load()
        {
            Components.ParentEntityId = Id;
            Classifications.ParentEntityId = Id;
            Locations.ParentEntityId = Id;
            Notes.ParentEntityId = Id;

            Components.Init();
            Classifications.Init();
            Locations.Init();
            Notes.Init();

            return base.Init();
        }
    }

    public interface IEditableGrid<TGridEntity> : IDotvvmViewModel
    {
        GridViewDataSet<TGridEntity> DataSet { get; }
        TGridEntity EditRow { get; }
        TGridEntity NewRow { get; }

        void Edit(int id);
        void Delete(int id);
        void Add();
        void Save();
    }

    public class EditableGrid<TGridEntity> : DotvvmViewModelBase, IEditableGrid<TGridEntity>
        where TGridEntity : class, IEntity<int>
    {
        private readonly ICrudFilteredFacade<TGridEntity, TGridEntity, IdFilter, int> facade;

        [Bind(Direction.None)]
        public int ParentEntityId { get; set; }

        public GridViewDataSet<TGridEntity> DataSet { get; private set; }
        public TGridEntity EditRow { get; private set; }
        public TGridEntity NewRow { get; private set; }

        public EditableGrid(ICrudFilteredFacade<TGridEntity, TGridEntity, IdFilter, int> facade)
        {
            this.facade = facade;
        }

        public void Add()
        {
            NewRow = facade.InitializeNew();
        }

        public void Edit(int id)
        {
            EditRow = facade.GetDetail(id);
            DataSet.RowEditOptions.EditRowId = id;
        }

        public void Delete(int id)
        {
            facade.Delete(id);
        }

        public void Save()
        {
            if (NewRow != null)
            {
                facade.Save(NewRow);
                NewRow = null;
            }
            else if (EditRow != null)
            {
                facade.Save(EditRow);
                EditRow = null;
            }
        }

        public override Task Init()
        {
            DataSet = new GridViewDataSet<TGridEntity>()
            {
                PagingOptions = { PageSize = 10},
                SortingOptions = new SortingOptions { },
                RowEditOptions = new RowEditOptions
                {
                    PrimaryKeyPropertyName = "Id"
                }
            };
            return base.Init();
        }

        public override Task PreRender()
        {

            facade.FillDataSet(DataSet, new IdFilter { Id = ParentEntityId });
            return base.PreRender();
        }
    }
}
