﻿using BcsAdmin.DAL.Models;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riganti.Utils.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Dto;

namespace BcsAdmin.BL.Queries
{
    public class EntitySuggestionQuery : EntityFrameworkQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        public EntitySuggestionQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<SuggestionDto> GetQueryable()
        {

            var context = Context.CastTo<AppDbContext>();

            var queriable =
                    context
                    .EpEntity
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter?.SearchText))
            {
                queriable = queriable.Where(e
                    => (e.Code?? "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1
                    || (e.Name?? "").IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }

            return queriable
                .ToList()
                .AsQueryable()
                .OrderBy(e => e.Code)
                .Take(10)
                .Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Name,
                    Name = e.Code
                });

        }
    }

    public class ClassificationSuggestionQuery : EntityFrameworkQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        public ClassificationSuggestionQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<SuggestionDto> GetQueryable()
        {

            var context = Context.CastTo<AppDbContext>();

            var queriable =
                    context
                    .EpClassification
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                queriable = queriable
                    .Where(e => e.Name != null)
                    .Where(e => e.Name.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }

            return queriable
                .OrderBy(e => e.Name)
                .Take(10)
                .Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Type,
                    Name = e.Name
                });
        }
    }

    public class OrganismSuggestionQuery : EntityFrameworkQuery<SuggestionDto>, IFilteredQuery<SuggestionDto, SuggestionFilter>
    {
        public SuggestionFilter Filter { get; set; }

        public OrganismSuggestionQuery(IUnitOfWorkProvider unitOfWorkProvider)
            : base(unitOfWorkProvider)
        {
        }

        protected override IQueryable<SuggestionDto> GetQueryable()
        {
            var context = Context.CastTo<AppDbContext>();

            var queriable =
                    context
                    .EpOrganism
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                queriable = queriable.Where(e
                    => e.Code.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1
                    || e.Name.IndexOf(Filter.SearchText, StringComparison.OrdinalIgnoreCase) != -1);
            }

            return queriable
                .OrderBy(e => e.Code)
                .Take(10)
                .Select(e => new SuggestionDto
                {
                    Id = e.Id,
                    Description = e.Name,
                    Name = e.Code
                });
        }
    }
}