using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public class EditableLinkGrid<TGridEntity, TSuggestionQuery> : DotvvmViewModelBase, IEditableLinkGrid<TGridEntity, TSuggestionQuery>
        where TGridEntity : class, IEntity<int>, IAssociatedEntity
        where TSuggestionQuery : IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        private readonly ILinkGridFacade<TGridEntity> facade;

        [Bind(Direction.None)]
        public SuggestionsFacade<TSuggestionQuery> SuggestionsFacade { get; }

        [Bind(Direction.Both)]
        public int ParentEntityId { get; set; }

        public GridViewDataSet<TGridEntity> DataSet { get; set; }
        public TGridEntity NewRow { get; set; }

        public EntitySearchSelect EntitySearchSelect { get; set; }

        public EditableLinkGrid(ILinkGridFacade<TGridEntity> facade, EntitySearchSelect entitySearchSelect, SuggestionsFacade<TSuggestionQuery> suggestionsFacade)
        {
            this.facade = facade;
            this.EntitySearchSelect =  entitySearchSelect;
            this.SuggestionsFacade = suggestionsFacade;
            EntitySearchSelect.SuggestionProvider = suggestionsFacade.GetSuggestions;
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
            facade.Unlink(entity.DetailEntityId ?? -1);
        }

        public void Cancel()
        {
            NewRow = null;
            DataSet.RowEditOptions.EditRowId = null;
        }

        public void SaveNew()
        {
            facade.CreateAndLink(NewRow, ParentEntityId);
            NewRow = null;
        }

        public void SaveEdit(TGridEntity entity)
        {
            facade.Edit(entity);
            DataSet.RowEditOptions.EditRowId = null;
        }

        public void Link()
        {
            var associateId = EntitySearchSelect?.SelectedLink?.Id;

            if (associateId == null) return;

            facade.Link(new EntityLinkDto {DetailId= ParentEntityId, AssociatedId= associateId.Value});
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
