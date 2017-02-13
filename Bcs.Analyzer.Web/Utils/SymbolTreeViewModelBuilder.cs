using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BcsAnalysisWeb.ViewModels;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel.Tree;

namespace BcsAnalysisWeb.Utils
{
    public class SymbolTreeViewModelBuilder
    {
        public TreeNode<SymbolTreeViewModel> Visit(BcsSymbol symbol)
        {
            var namedSymbol = symbol.As<BcsNamedSymbol>();
            return new TreeNode<SymbolTreeViewModel>
            {
                Data = new SymbolTreeViewModel
                {
                    Name = namedSymbol?.Name ?? "-",
                    Type = symbol.Type.ToString(),
                    Display = namedSymbol.ToDisplayString()
                },
                Children = symbol.EnumerateChildNodes().Select(Visit).ToList()
            };
        }
    }
}