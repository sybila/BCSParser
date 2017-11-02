using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BcsAnalysisWeb.ViewModels;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.BoundTree;
using Bcs.Analyzer.DemoWeb.ViewModels;

namespace Bcs.Analyzer.DemoWeb.Utils
{
    public class SemanticTreeViewModelBuilder
    {
        public TreeNode<SemanticNodeViewModel> Visit(IBcsBoundSymbol symbol)
        {
            if (symbol is IBcsComposedBoundSymbol)
            {
                var cs = symbol.CastTo<IBcsComposedBoundSymbol>();
                return new TreeNode<SemanticNodeViewModel>
                {
                    Data = CreateSymbolVM(symbol),
                    Children = cs.StatedContent.Values.SelectMany(c => c.Select(Visit)).ToList()
                };
            }
            if (symbol is BcsBoundLocation)
            {
                var ls = symbol.CastTo<BcsBoundLocation>();
                return new TreeNode<SemanticNodeViewModel>
                {
                    Data = CreateSymbolVM(symbol),
                    Children = new List<TreeNode<SemanticNodeViewModel>> { Visit(ls.Content) }
                };
            }
            if (symbol.Is<BcsBoundVariableExpression>())
            {
                var ve = symbol.CastTo<BcsBoundVariableExpression>();
                return new TreeNode<SemanticNodeViewModel>()
                {
                    Data = CreateSymbolVM(symbol),
                    Children = new List<TreeNode<SemanticNodeViewModel>> { Visit(ve.Target) }
                };
            }
            return new TreeNode<SemanticNodeViewModel>
            {
                Data = CreateSymbolVM(symbol),
                Children = new List<TreeNode<SemanticNodeViewModel>>()
            };
        }

        private static SemanticNodeViewModel CreateSymbolVM(IBcsBoundSymbol symbol)
        {
            return new SemanticNodeViewModel
            {
                Symbol = $"{symbol?.Symbol?.GetType().Name}: {symbol?.Symbol?.ToDisplayString()}",
                SyntaxNode = $"{symbol?.Syntax?.GetType().Name}: {symbol?.Syntax?.ToDisplayString()}",
                Type = symbol?.GetType().Name
            };
        }
    }
}