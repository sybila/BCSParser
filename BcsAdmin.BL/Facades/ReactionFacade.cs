﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bcs.Admin.BL.Dto;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Services;
using BcsAdmin.DAL.Api;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.Syntax;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace BcsAdmin.BL.Facades
{
    public class ReactionFacade : FilteredCrudFacadeBase<ApiRule, int, ReactionRowDto, BiochemicalReactionDetailDto, ReactionFilter>, IListFacade<ReactionRowDto, ReactionFilter>
    {
        private readonly IBcsWorkspace bcsWorkspace;

        public ReactionFacade(
            Func<IFilteredQuery<ReactionRowDto, ReactionFilter>> queryFactory,
            IRepository<ApiRule, int> repository,
            IEntityDTOMapper<ApiRule, BiochemicalReactionDetailDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider,
            IBcsWorkspace bcsWorkspace)
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;

            this.bcsWorkspace = bcsWorkspace;
            
        }

        public List<StyleSpan> GetClassificationSpans(ReactionModel reactionModel)
        {
            if(reactionModel == null) { return new List<StyleSpan>(); } 

            var semanticColorVisitor = new SemanticColoringVisitor();

            semanticColorVisitor.Visit(reactionModel.SemanticTree);
            var spans = semanticColorVisitor.SemanticStyleSpans;

            foreach (var error in reactionModel.Errors)
            {
                var errorTag = new StyleSpan
                {
                    Range = error.TextRange,
                    CssClass = "error-tag",
                    TooltipText = error.Message
                };

                spans.Add(errorTag);
            }
            return spans;
        }

        public async Task<ReactionModel> GetReactionModelAsync(string sourceText)
        {
            await bcsWorkspace.CreateSemanticModelAsync(new System.Threading.CancellationToken());

            var provider = new WorkspaceModelProvider(bcsWorkspace);
            var reactionModel = provider.CreateReactionModel(sourceText);
            return reactionModel;
        }
    }
}
