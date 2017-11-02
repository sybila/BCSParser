using System.Linq;
using BcsAnalysisWeb.ViewModels;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Visitors;
using Bcs.Analyzer.DemoWeb.ViewModels;

namespace Bcs.Analyzer.DemoWeb.Utils
{ 
    public class SyntaxTreeViewModelBuilder : BcsExpressionBuilderVisitor<TreeNode<SyntaxNodeViewModel>, object>
    {
        protected override TreeNode<SyntaxNodeViewModel> VisitDefault(BcsExpressionNode node, object parameter)
        {
            return new TreeNode<SyntaxNodeViewModel>
            {
                Data = new SyntaxNodeViewModel
                {
                    Dispaly = node.ToDisplayString(),
                    NodeName = node.GetType().Name,
                    Errors = node.Errors.Select(e=> e.Message).ToList()
                },
                Children = node.EnumerateChildNodes().Select(n => Visit(n, parameter)).ToList()
            };
        }
    }

}