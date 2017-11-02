using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Extensions;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.BoundTree;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using MyTreeNode = Bcs.Analyzer.DemoWeb.ViewModels.TreeNode<string>;
using Bcs.Analyzer.DemoWeb.Utils;
using Bcs.Analyzer.DemoWeb.ViewModels;
using BcsAdmin.BL.Services;

namespace BcsAnalysisWeb.ViewModels
{
    public class DefaultViewModel : DotvvmViewModelBase
    {
        private static Dictionary<Guid, BcsReactionNode> reactions;
        private static BcsDefinitionFile document;
        private static IBcsWorkspace workspace;

        public TextPresenter TextPresenter { get; set; } = new TextPresenter();

        public List<TreeNode<SyntaxNodeViewModel>> SyntaxToDraw { get; set; } = new List<TreeNode<SyntaxNodeViewModel>>();
        public List<TreeNode<SemanticNodeViewModel>> SemanticToDraw { get; set; } = new List<TreeNode<SemanticNodeViewModel>>();
        public List<SemanticErrorViewModel> SemanticErrors { get; set; } = new List<SemanticErrorViewModel>();
        public List<ReactionViewModel> Reactions { get; set; } = new List<ReactionViewModel>();

        [Bind(Direction.ServerToClient)]
        public string ErrorMessage { get; set; }

        public GridViewDataSet<EntityViewModel> EntityDataSet { get; set; }

        public string TextEdit { get; set; }

        public string Title { get; set; }

        static DefaultViewModel()
        {
            LoadBcsFile("D:\\yamada.txt");
            if (document != null)
            {
                workspace = new DbWorkspace();
                workspace.CreateSemanticModel();
            }
        }

        public DefaultViewModel()
        {
            EntityDataSet = new GridViewDataSet<EntityViewModel>()
            {
                PageSize = 20,
                Items = workspace.GetAllEntities()
                    .Select(e => new EntityViewModel
                    {
                        Name = e.Name,
                        Type = e.Type.GetDescription(),
                        Children = e.Parts.Select(p => $"[{p.Type.GetDescription()}: {p.Name}]").ToList()
                    })
                    .ToList()
            };
            Title = "Hello from BCS!";
        }

        public override Task Init()
        {
            if (document != null)
            {
                Reactions = reactions.Select(r => new ReactionViewModel { Id = r.Key, Display = r.Value.ToDisplayString() }).ToList();
            }
            return base.Init();
        }

        public void DrawSemanticTree(Guid id)
        {
            var reaction = reactions[id];
            ClearTrees();

            var semanticAnalyzer = new SemanticAnalisisVisitor(workspace, new BcsBoundSymbolFactory());
            var semanticTree = semanticAnalyzer.Visit(reaction);

            DrawSemanticTree(semanticTree, semanticAnalyzer.Errors);
        }

        public void DrawTree(Guid id)
        {
            ClearTrees();

            var viewModelTreeBuilder = new SyntaxTreeViewModelBuilder();
            SyntaxToDraw.Add(viewModelTreeBuilder.Visit(reactions[id]));
        }

        public void Click(TreeNode<SyntaxNodeViewModel> item)
        {
            item.Children.Add(new TreeNode<SyntaxNodeViewModel>()
            {
                Data = new SyntaxNodeViewModel()
                {
                    Dispaly = "Delf",
                    NodeName = "Delf",
                    Errors = new List<string>()
                },
                Children = new List<TreeNode<SyntaxNodeViewModel>>()
            });
        }

        public void DrawLive()
        {
            var semanticAnalyzer = new SemanticAnalisisVisitor(workspace, new BcsBoundSymbolFactory());
            var semanticColorVisitor = new SemanticColoringVisitor();

            var rawText = TextPresenter.ToRawText(TextEdit);
            TextEdit = TextPresenter.CreateRichText(rawText, new List<StyleSpan>());

            var tree = BcsSyntaxFactory.ParseReaction(rawText);
            if (tree == null) { return; }

            var semanticTree = semanticAnalyzer.Visit(tree);
            if (semanticTree == null) { return; }

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
            TextEdit = TextPresenter.CreateRichText(rawText, spans);



            ClearTrees();

            var viewModelTreeBuilder = new SyntaxTreeViewModelBuilder();
            SyntaxToDraw.Add(viewModelTreeBuilder.Visit(tree));

            DrawSemanticTree(semanticTree, semanticAnalyzer.Errors);
        }

        private void DrawSemanticTree(IBcsBoundSymbol semanticTree, Dictionary<BcsExpressionNode, List<SemanticError>> semanticAnalyzerErrors)
        {
            var semanticVmBuilder = new SemanticTreeViewModelBuilder();

            var vmTree = semanticVmBuilder.Visit(semanticTree);

            SemanticToDraw.Add(vmTree);

            foreach (var error in semanticAnalyzerErrors)
            {
                var errs = error.Value.Select(e => new SemanticErrorViewModel
                {
                    Message = e.Message,
                    AssociatedSyntax = error.Key.ToDisplayString()
                });
                SemanticErrors.AddRange(errs);
            }
        }

        private void ClearTrees()
        {
            SyntaxToDraw.Clear();
            SemanticToDraw.Clear();
            SemanticErrors.Clear();
        }



        private static void LoadBcsFile(string fileName)
        {
            try
            {
                using (var bcsHandler = new BcsDefinitionFileReader())
                {
                    document = bcsHandler.ReadFile(fileName);
                }

                reactions = document.Rules
                   .Select(r => BcsSyntaxFactory.ParseReaction(r.Equation))
                   .Cast<BcsReactionNode>()
                   .ToDictionary(key => key.UniqueId);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
