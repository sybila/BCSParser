using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using DotVVM.Framework.ViewModel;

namespace Bcs.Admin.Web.ViewModels
{
    public class SearchSelect : DotvvmViewModelBase
    {
        [Bind(Direction.None)]
        public Func<SuggestionFilter, IList<SuggestionDto>> SuggestionProvider { get; set; }

        public IList<SuggestionDto> Suggestions { get; set; } = new List<SuggestionDto>();

        public SuggestionDto SelectedSuggestion { get; set; }

        public SuggestionFilter Filter { get; set; } = new SuggestionFilter();

        public void Select(SuggestionDto link)
        {
            SelectedSuggestion = link;
            Filter.SearchText = link.Name;
        }

        public void RefreshSuggestions()
        {
            Suggestions = SuggestionProvider(Filter);
            SelectedSuggestion = null;
        }
    }
}
