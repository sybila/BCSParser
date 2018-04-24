using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Queries;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace BcsAdmin.BL.Facades
{
    public class SuggestionsFacade<TQuery> : FacadeBase
        where TQuery : IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionsFacade(IUnitOfWorkProvider unitOfWorkProvider, Func<TQuery> entitySuggestionQuery)
        {
            UnitOfWorkProvider = unitOfWorkProvider;
            EntitySuggestionQuery = entitySuggestionQuery;
        }

        public Func<TQuery> EntitySuggestionQuery { get; }

        public async System.Threading.Tasks.Task<IList<SuggestionDto>> GetSuggestionsAsync(SuggestionFilter filter)
        {
                var q = EntitySuggestionQuery();
                q.Filter = filter;
                return await q.ExecuteAsync();
            
        }
    }
}
