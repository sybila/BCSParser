using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels.Grids
{
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
