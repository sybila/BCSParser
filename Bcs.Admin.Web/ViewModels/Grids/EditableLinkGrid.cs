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
    public class EditableLinkGrid<TGridEntity, TSuggestionQuery> : AppViewModelBase, IEditableLinkGrid<TGridEntity, TSuggestionQuery>
        where TGridEntity : class, IEntity<int>, IAssociatedEntity
        where TSuggestionQuery : IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        private readonly ILinkGridFacade<TGridEntity> facade;

        public bool IsCollapsed { get; set; }

        [Bind(Direction.None)]
        public SuggestionsFacade<TSuggestionQuery> SuggestionsFacade { get; }

        [Bind(Direction.Both)]
        public int ParentEntityId { get; set; }
        public string ParentRepositoryName { get; set; }

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
            EntitySearchSelect.SuggestionProvider = suggestionsFacade.GetSuggestionsAsync;
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

        public async Task DeleteAsync(TGridEntity entity)
        {
            await ExecuteSafeAsync(async () =>
            {
                facade.UnlinkAsync(ParentRepositoryName, new EntityLinkDto { DetailId = ParentEntityId, AssociatedId = entity.Id });
                await ReloadDataAsync();
            });
        }

        public void Cancel()
        {
            NewRow = null;
            DataSet.RowEditOptions.EditRowId = null;
        }

        public async Task SaveNewAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                facade.CreateAndLinkAsync(ParentEntityId, ParentRepositoryName, NewRow);
                NewRow = null;
                await ReloadDataAsync();
            });
        }

        public async Task SaveEditAsync(TGridEntity entity)
        {
            await ExecuteSafeAsync(async () =>
            {
                facade.EditAsync(entity);
                DataSet.RowEditOptions.EditRowId = null;
                await ReloadDataAsync();
            });
        }

        public async Task LinkAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                var associateId = EntitySearchSelect?.SelectedSuggestion?.Id;

                if (associateId == null) return;

                facade.LinkAsync(ParentRepositoryName, new EntityLinkDto { DetailId = ParentEntityId, AssociatedId = associateId.Value });
                await ReloadDataAsync();
            });
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

        public async Task ReloadDataAsync()
        {
            await ExecuteSafeAsync(async () =>
            {
                await facade.FillDataSetAsync(DataSet, new IdFilter { Id = ParentEntityId, ParentEntityType = ParentRepositoryName });
            });
        }
    }
}
