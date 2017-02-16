using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BcsAnalysisWeb.ViewModels;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel.Tree;

namespace BcsAnalysisWeb.Utils
{
    public class SymbolTreeViewModelBuilder : BcsSymbolVisitor<TreeNode<SymbolTreeViewModel>, object>
    {
        protected  override TreeNode<SymbolTreeViewModel> VisitDefault(BcsSymbol symbol, object param)
        {
            var namedSymbol = symbol.As<BcsNamedSymbol>();
            return new TreeNode<SymbolTreeViewModel>
            {
                Data = new SymbolTreeViewModel
                {
                    Name = namedSymbol?.Name ?? "-",
                    Type = symbol.Type.ToString(),
                    Display = symbol.ToDisplayString()
                },
                Children = symbol.EnumerateChildNodes().Select(c=> Visit(c, null)).ToList()
            };
        }
    }
}