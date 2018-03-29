using BcsAdmin.BL.Dto;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Queries;

namespace Bcs.Admin.Web.ViewModels
{
    public class SimilarEntitySearchPanel : DotvvmViewModelBase
    {
        public readonly AdvancedEntitySearchFacade searchFacade;
        private readonly SuggestionsFacade<EntitySuggestionQuery> suggestionsFacade;

        public GridViewDataSet<SimilarEntityDto> DataSet { get; set; }

        public SearchSelect EntityToCheckSearchSelect { get; set; }

        public SimilarEntityDto CheckedEntity { get; set; }
        public string DatabaseName { get; set; }

        public string Message { get; set; }

        public SimilarEntitySearchPanel(AdvancedEntitySearchFacade searchFacade, SuggestionsFacade<EntitySuggestionQuery> suggestionsFacade, SearchSelect searchSelect)
        {
            this.searchFacade = searchFacade;
            this.suggestionsFacade = suggestionsFacade;
            EntityToCheckSearchSelect = searchSelect;
            EntityToCheckSearchSelect.SuggestionProvider = suggestionsFacade.GetSuggestions;
            EntityToCheckSearchSelect.Filter.AllowedEntityTypes = new[] {
                HierarchyType.Atomic,
                HierarchyType.Compartment,
                HierarchyType.Complex,
                HierarchyType.State,
                HierarchyType.Structure
            };
        }

        public void Search()
        {
            var filter = new SimilarEntitySearchFilter
            {
                Name = EntityToCheckSearchSelect.SelectedSuggestion?.Name,
                DatabaseName = DatabaseName
            };

            searchFacade.FillSimilarEntitySearch(DataSet, filter);
        }

        public void ReplaceEntyty()
        {
            Message = "Entity replaced... wel not really this is prototype.";
        }

        public override Task Init()
        {
            if (!Context.IsPostBack)
            {
                DataSet = new GridViewDataSet<SimilarEntityDto>()
                {
                    PagingOptions = { PageSize = 1 },
                    SortingOptions = new SortingOptions { },
                    RowEditOptions = new RowEditOptions
                    {
                        PrimaryKeyPropertyName = nameof(SimilarEntityDto.Name)
                    }
                };
            }
            return base.Init();
        }
    }
}
