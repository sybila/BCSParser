using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Facades;
using DotVVM.Framework.ViewModel;

namespace Bcs.Admin.Web.ViewModels
{
    public class EntitySearchSelect : DotvvmViewModelBase
    {
        [Bind(Direction.None)]
        public Func<string ,IList<SuggestionDto>> SuggestionProvider { get; set; }

        public SuggestionDto SelectedSuggestion { get; set; }

        public IList<SuggestionDto> Suggestions { get; set; } = new List<SuggestionDto>();

        public SuggestionDto SelectedLink { get; set; }

        public string Text { get; set; }

        public void Select(SuggestionDto link)
        {
            SelectedLink = link;
            Text = link.Name;
        }

        public void RefreshSuggestions()
        {
            Suggestions = SuggestionProvider(Text);
            SelectedLink = null;
        }
    }
}
