using BcsAdmin.BL.Dto;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure;
using Riganti.Utils.Infrastructure.Services.Facades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bcs.Admin.Web.ViewModels.Grids;

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
        public IEditableGrid<ComponentLinkDto> Components { get; set; }

        [Display(AutoGenerateField = false)]
        public IEditableGrid<LocationLinkDto> Locations { get; set; }

        [Display(AutoGenerateField = false)]
        public IEditableGrid<ClassificationDto> Classifications { get; set; }
        public bool IsVisible { get; internal set; }

        //[Display(AutoGenerateField = false)]
        //public EditableGrid<EntityNoteDto> Notes { get; set; }

        public BiochemicalEntityDetail(
          IEditableGrid<ComponentLinkDto> componentGrid,
          IEditableGrid<LocationLinkDto> locationGrid,
          IEditableGrid<ClassificationDto> classificationGrid
          /*EditableGrid<EntityNoteDto> noteGrid*/)
        {
            Components = componentGrid;
            Locations = locationGrid;
            Classifications = classificationGrid;
            //Notes = noteGrid;
        }

        public void PoputateGrids()
        {
            Components.ParentEntityId = Id;
            Classifications.ParentEntityId = Id;
            Locations.ParentEntityId = Id;
            //Notes.ParentEntityId = Id;

            Components.Init();
            Classifications.Init();
            Locations.Init();
            //Notes.Init();
        }
    }
}
