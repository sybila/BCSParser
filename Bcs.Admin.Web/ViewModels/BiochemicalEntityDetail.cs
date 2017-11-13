using BcsAdmin.BL.Dto;
using DotVVM.Framework.Controls;
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
    public class BiochemicalEntityDetail
    {
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
        public List<BiochemicalEntityTypeDto> HierarchyTypes { get; set; }

        [Display(AutoGenerateField = false)]
        public BiochemicalEntityLinkDto Parent { get; set; }

        [Display(AutoGenerateField = false)]
        public IList<BiochemicalEntityLinkDto> Components { get; set; }

        [Display(AutoGenerateField = false)]
        public IList<BiochemicalEntityLinkDto> Locations { get; set; }

        [Display(AutoGenerateField = false)]
        public IList<ClassificationDto> Classifications { get; set; }

        [Display(AutoGenerateField = false)]
        public IList<EntityNoteDto> Notes { get; set; }
    }

    public interface IEditableGrid<TGridEntity>
    {
        GridViewDataSet<TGridEntity> DataSet { get; set; }

        TGridEntity EditRow { get; set; }
        TGridEntity NewRow { get; set; }

        void Edit(int id);
        void Delete(int id);
        void Add();
        void Save();
    }



    public class EditableGrid<TGridEntity> : DotvvmViewModelBase, IEditableGrid<TGridEntity>
        where TGridEntity : class, IEntity<int>
    {
        private readonly ICrudFacade<TGridEntity, TGridEntity, int> facade;

        public EditableGrid(ICrudFacade<TGridEntity, TGridEntity, int> facade)
        {
            this.facade = facade;
        }

        public GridViewDataSet<TGridEntity> DataSet { get; set; }
        public TGridEntity EditRow { get; set; }
        public TGridEntity NewRow { get; set; }

        public void Add()
        {
            NewRow = facade.InitializeNew();
        }

        public void Edit(int id)
        {
            EditRow = facade.GetDetail(id);
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
                PagingOptions = { },
                SortingOptions = new SortingOptions { }
            };
            return base.Init();
        }

        public override Task PreRender()
        {
            if (!Context.IsPostBack || DataSet.IsRefreshRequired)
            {
                facade.FillDataSet(DataSet);
            }
            return base.PreRender();
        }
    }
}
