using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public class EditableGrid<TGridEntity> : DotvvmViewModelBase, IEditableGrid<TGridEntity>
        where TGridEntity : class, IEntity<int>, IManyToManyEntity
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

        public void Edit(TGridEntity entity)
        {
            NewRow = null;
            DataSet.RowEditOptions.EditRowId = entity.Id;
        }

        public void Delete(TGridEntity entity)
        {
            facade.Unlink(entity.IntermediateEntityId ?? -1);
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
            facade.Edit(NewRow);
            DataSet.RowEditOptions.EditRowId = null;
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
