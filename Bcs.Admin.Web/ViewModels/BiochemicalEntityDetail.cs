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

        //[Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        //[Required]
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

        public void PoputateGrids()
        {
            Components.ParentEntityId = Id;
            Classifications.ParentEntityId = Id;
            Locations.ParentEntityId = Id;
            Notes.ParentEntityId = Id;

            Components.Init();
            Classifications.Init();
            Locations.Init();
            Notes.Init();
        }
    }

    public interface IEditableGrid<TGridEntity> : IDotvvmViewModel
    {
        GridViewDataSet<TGridEntity> DataSet { get; }
        TGridEntity NewRow { get; }

        void Edit(int id);
        void Delete(int id);
        void Add();
        void SaveEdit(TGridEntity entity);
        void SaveNew();
    }

    public class EditableGrid<TGridEntity> : DotvvmViewModelBase, IEditableGrid<TGridEntity>
        where TGridEntity : class, IEntity<int>
    {
        private readonly IGridFacade<TGridEntity> facade;

        [Bind(Direction.Both)]
        public int ParentEntityId { get; set; }

        public GridViewDataSet<TGridEntity> DataSet { get; set; }
        public TGridEntity NewRow { get; set; }

        public EditableGrid(IGridFacade<TGridEntity> facade)
        {
            this.facade = facade;
        }

        public void Add()
        {
            NewRow = facade.CreateAssociated();
        }

        public void Edit(int id)
        {
            NewRow = null;
            DataSet.RowEditOptions.EditRowId = id;
        }

        public void Delete(int id)
        {
            facade.Unlink(id);
        }

        public void Cancel()
        {
            NewRow = null;
            DataSet.RowEditOptions.EditRowId = null;
        }

        public void SaveNew()
        {
            facade.CreateAndLink(NewRow, ParentEntityId);
        }

        public void SaveEdit(TGridEntity entity)
        {
            facade.Edit(entity);
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
