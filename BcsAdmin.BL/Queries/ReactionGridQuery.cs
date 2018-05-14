using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Api;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class ReactionGridQuery : AppApiQuery<ReactionRowDto>, IFilteredQuery<ReactionRowDto, ReactionFilter>
    {
        private readonly IMapper mapper;

        public ReactionFilter Filter { get; set; }

        public ReactionGridQuery(IMapper mapper)
            : base()
        {
            this.mapper = mapper;
        }

        protected async override Task<IQueryable<ReactionRowDto>> GetQueriableAsync(CancellationToken cancellationToken)
        {
            var queriable = await GetWebDataAsync<ApiRule>(cancellationToken, "rules");

            if (!string.IsNullOrWhiteSpace(Filter?.SearchText))
            {
                queriable = queriable.Where(e => (e.Equation ?? "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }

            return queriable.ProjectTo<ReactionRowDto>(mapper.ConfigurationProvider);
        }
    }
}
