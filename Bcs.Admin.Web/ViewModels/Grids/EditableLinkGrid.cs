using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure.Core;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public class EditableLinkGrid<TGridEntity, TSuggestionQuery> : DotvvmViewModelBase, IEditableLinkGrid<TGridEntity, TSuggestionQuery>
        where TGridEntity : class, IEntity<int>, IAssociatedEntity
        where TSuggestionQuery : IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        private readonly ILinkGridFacade<TGridEntity> facade;

        public bool IsCollapsed { get; set; }

        [Bind(Direction.None)]
        public SuggestionsFacade<TSuggestionQuery> SuggestionsFacade { get; }

        [Bind(Direction.Both)]
        public int ParentEntityId { get; set; }

        public GridViewDataSet<TGridEntity> DataSet { get; set; }
        public TGridEntity NewRow { get; set; }

        public SearchSelect EntitySearchSelect { get; set; }

        [Bind(Direction.ServerToClient)]
        public int ItemsCount => DataSet.PagingOptions.TotalItemsCount;

        public EditableLinkGrid(ILinkGridFacade<TGridEntity> facade, SearchSelect entitySearchSelect, SuggestionsFacade<TSuggestionQuery> suggestionsFacade)
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
            facade.Unlink(entity.IntermediateEntityId ?? -1);
            DataSet.RequestRefresh(true);
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
            DataSet.RequestRefresh(true);
        }

        public void SaveEdit(TGridEntity entity)
        {
            facade.Edit(entity);
            DataSet.RowEditOptions.EditRowId = null;
            DataSet.RequestRefresh(true);
        }

        public void Link()
        {
            var associateId = EntitySearchSelect?.SelectedSuggestion?.Id;

            if (associateId == null) return;

            facade.Link(new EntityLinkDto {DetailId= ParentEntityId, AssociatedId= associateId.Value});
            DataSet.RequestRefresh(true);
        }

        public override Task Init()
        {
            if (!Context.IsPostBack)
            {
                DataSet = new GridViewDataSet<TGridEntity>()
                {
                    PagingOptions = new PagingOptions { PageSize = 100 },
                    SortingOptions = new SortingOptions { },
                    RowEditOptions = new RowEditOptions
                    {
                        PrimaryKeyPropertyName = "Id"
                    }
                };
            }
            return base.Init();
        }

        public override Task Load()
        {
            DataSet.OnLoadingData = options => facade.GetData(options, new IdFilter { Id = ParentEntityId });
            return base.Load();
        }
    }
}
