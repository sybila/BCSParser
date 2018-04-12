using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bcs.Admin.BL.Dto;
using BcsAdmin.BL.Dto;
using BcsAdmin.BL.Filters;
using BcsAdmin.BL.Services;
using BcsAdmin.DAL.Models;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.Syntax;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFrameworkCore;
using Riganti.Utils.Infrastructure.Services.Facades;

namespace BcsAdmin.BL.Facades
{
    public class ReactionFacade : FilteredCrudFacadeBase<EpReaction, int, ReactionRowDto, BiochemicalReactionDetailDto, ReactionFilter>, IListFacade<ReactionRowDto, ReactionFilter>
    {
        private readonly IBcsWorkspace bcsWorkspace;

        public ReactionFacade(
            Func<IFilteredQuery<ReactionRowDto, ReactionFilter>> queryFactory,
            IRepository<EpReaction, int> repository,
            IEntityDTOMapper<EpReaction, BiochemicalReactionDetailDto> mapper,
            IUnitOfWorkProvider unitOfWorkProvider,
            IBcsWorkspace bcsWorkspace)
            : base(queryFactory, repository, mapper)
        {
            UnitOfWorkProvider = unitOfWorkProvider;

            this.bcsWorkspace = bcsWorkspace;
            bcsWorkspace.CreateSemanticModel();
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

        public ReactionModel GetReactionModel(string sourceText)
        {
            var provider = new WorkspaceModelProvider(bcsWorkspace);
            var reactionModel = provider.CreateReactionModel(sourceText);
            return reactionModel;
        }
    }
}
