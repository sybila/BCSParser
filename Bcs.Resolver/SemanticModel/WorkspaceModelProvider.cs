using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BcsResolver.Syntax;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.SemanticModel
{
    public class WorkspaceModelProvider
    {
        public IBcsWorkspace Workspace { get; }
        public SemanticAnalysisVisitor AnalysisVisitor { get; }

        public WorkspaceModelProvider(IBcsWorkspace workspace)
        {
            Workspace = workspace;
            AnalysisVisitor = new SemanticAnalysisVisitor(workspace, new BcsBoundSymbolFactory());
        }

        public ReactionModel CreateReactionModel(string sourceText)
        {
            var tree = BcsSyntaxFactory.ParseReaction(sourceText);

            if(tree == null) { return null; }

            var semanticTree = AnalysisVisitor.Visit(tree);

            var syntaxErrors = 
                tree.Errors
                .Select(e=> new ModelError(ModelErrorType.Syntax, e.Message, e.Range));

            var semanticErrors = AnalysisVisitor.Errors
                .SelectMany(eg => eg.Value.Select(e=> new ModelError(ModelErrorType.Semantic, e.Message, eg.Key.ExpressioRange)));

            var errors = syntaxErrors
                .Concat(semanticErrors)
                .ToList();

            return new ReactionModel(sourceText, tree, semanticTree, errors);
        }
    }
}
