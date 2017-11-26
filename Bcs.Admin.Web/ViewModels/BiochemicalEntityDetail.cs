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
using BcsAdmin.BL.Facades;
using AutoMapper;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalEntityDetail : DotvvmViewModelBase
    {
        private readonly DashboardFacade dashboardFacade;
        private readonly IMapper mapper;

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
        public IEditableGrid<ComponentLinkDto> Components { get; set; }

        [Display(AutoGenerateField = false)]
        public IEditableGrid<LocationLinkDto> Locations { get; set; }

        [Display(AutoGenerateField = false)]
        public IEditableGrid<ClassificationDto> Classifications { get; set; }

        //[Display(AutoGenerateField = false)]
        //public EditableGrid<EntityNoteDto> Notes { get; set; }

        public BiochemicalEntityDetail(
            DashboardFacade dashboardFacade,
            IMapper mapper,
            IEditableGrid<ComponentLinkDto> componentGrid,
            IEditableGrid<LocationLinkDto> locationGrid,
            IEditableGrid<ClassificationDto> classificationGrid
            /*EditableGrid<EntityNoteDto> noteGrid*/)
        {
            this.dashboardFacade = dashboardFacade;
            this.mapper = mapper;
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

        public void CancelAllActions()
        {
            Components.Cancel();
            Classifications.Cancel();
            Locations.Cancel();
            Components.Cancel();
        }

        public void Save()
        {
            var dto = mapper.Map<BiochemicalEntityDetailDto>(this);
            dashboardFacade.Save(dto);
        }
    }
}
