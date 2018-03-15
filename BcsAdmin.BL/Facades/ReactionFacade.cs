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

        public List<StyleSpan> GetClassificationSpans(string sourceText)
        {
            var semanticAnalyzer = new SemanticAnalisisVisitor(bcsWorkspace, new BcsBoundSymbolFactory());
            var semanticColorVisitor = new SemanticColoringVisitor();

            var tree = BcsSyntaxFactory.ParseReaction(sourceText);
            if (tree == null) { return new List<StyleSpan> { }; }

            var semanticTree = semanticAnalyzer.Visit(tree);
            if (semanticTree == null) { return new List<StyleSpan> { }; }

            semanticColorVisitor.Visit(semanticTree);

            var spans = semanticColorVisitor.SemanticStyleSpans;

            foreach (var error in semanticAnalyzer.Errors)
            {
                var errorTag = new StyleSpan
                {
                    Range = error.Key.ExpressioRange,
                    CssClass = "error-tag"
                };

                spans.Add(errorTag);
            }
            return spans;
        }
    }
}
