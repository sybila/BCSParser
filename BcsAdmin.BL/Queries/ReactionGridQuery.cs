using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;

namespace BcsAdmin.BL.Queries
{
    public class ReactionGridQuery : EntityFrameworkQuery<ReactionRowDto>, IFilteredQuery<ReactionRowDto, ReactionFilter>
    {
        private readonly IMapper mapper;

        public ReactionFilter Filter { get; set; }

        public ReactionGridQuery(IUnitOfWorkProvider unitOfWorkProvider, IMapper mapper)
            : base(unitOfWorkProvider)
        {
            this.mapper = mapper;
        }

        protected override IQueryable<ReactionRowDto> GetQueryable()
        {
            var queriable =
                  Context.CastTo<AppDbContext>()
                  .EpReaction
                  .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter?.SearchText))
            {
                if (!string.IsNullOrWhiteSpace(Filter.SearchText))
                {
                    queriable = queriable.Where(e
                        => (e.Code != null ? e.Code : "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1
                        || (e.Name != null ? e.Name : "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
                }
            }

            return queriable.ProjectTo<ReactionRowDto>(mapper.ConfigurationProvider);
        }
    }
}
