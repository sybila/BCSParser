using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Facades;
using BcsAdmin.BL.Filters;
using Riganti.Utils.Infrastructure.Core;

namespace Bcs.Admin.Web.ViewModels.Grids
{
    public interface IEditableLinkGrid<TGridEntity, TSuggestionQuery> : IEditableGrid<TGridEntity>
        where TGridEntity : class, IEntity<int>
        where TSuggestionQuery : IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        SuggestionsFacade<TSuggestionQuery> SuggestionsFacade { get; }

        SearchSelect EntitySearchSelect { get; set; }
        
        void Link();
    }
}
