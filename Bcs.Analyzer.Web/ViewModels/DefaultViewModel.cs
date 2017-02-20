using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using AngleSharp.Parser.Html;
using BcsAnalysisWeb.Controls;
using BcsAnalysisWeb.Utils;
using BcsResolver.Extensions;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax;
using BcsResolver.Syntax.Parser;
using DotVVM.Framework.Controls;
using DotVVM.Framework.ViewModel;
using Ganss.XSS;
using MyTreeNode = BcsAnalysisWeb.ViewModels.TreeNode<string>;

namespace BcsAnalysisWeb.ViewModels
{
    public class DefaultViewModel : DotvvmViewModelBase
    {
        private static Dictionary<Guid, BcsReactionNode> reactions;
        private static BcsDefinitionFile document;
        private static BcsWorkspace workspace;

        public List<TreeNode<SyntaxNodeViewModel>> SyntaxToDraw { get; set; } = new List<TreeNode<SyntaxNodeViewModel>>();
        public List<TreeNode<SemanticNodeViewModel>> SemanticToDraw { get; set; } = new List<TreeNode<SemanticNodeViewModel>>();
        public List<ReactionViewModel> Reactions { get; set; } = new List<ReactionViewModel>();

        public GridViewDataSet<EntityViewModel> EntityDataSet { get; set; }= new GridViewDataSet<EntityViewModel>()
        {
            PageSize = 20,
            Items = workspace.GetAllEntities()
            .Select(e=> new EntityViewModel
                {
                    Name = e.Name,
                    Type = e.Type.GetDescription(),
                    Children = e.Parts.Select(p => $"[{p.Type.GetDescription()}: {p.Name}]").ToList()
                })
            .ToList()
        };

        public string TextEdit { get; set; }

        public string Title { get; set; }

        static DefaultViewModel()
        {
            LoadBcsFile("D:\\yamada.txt");

            workspace = new BcsWorkspace(new BcsFileMetadataProvider(document));
            workspace.CreateSemanticModel();
        }

        public DefaultViewModel()
        {
            Title = "Hello from BCS!";
        }

        public override Task Init()
        {
            Reactions = reactions.Select(r=> new ReactionViewModel {Id = r.Key, Display = r.Value.ToDisplayString()}).ToList();

            return base.Init();
        }

        public void DrawSemanticTree(Guid id)
        {
            var semanticAnalyzer = new SemanticAnalisisVisitor(workspace, new BcsBoundSymbolFactory());
            var semanticVmBuilder= new SemanticTreeViewModelBuilder();
            var reaction = reactions[id];

            var semanticTree = semanticAnalyzer.Visit(reaction);

            var vmTree = semanticVmBuilder.Visit(semanticTree);

            SemanticToDraw.Clear();
            SemanticToDraw.Add(vmTree);
        }

        public void DrawTree(Guid id)
        {
            var viewModelTreeBuilder = new SyntaxTreeViewModelBuilder();

            SyntaxToDraw.Clear();

            SyntaxToDraw.Add(viewModelTreeBuilder.Visit(reactions[id]));
        }

        public void Click(TreeNode<SyntaxNodeViewModel> item)
        {
            throw new ArgumentNullException();
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
            var sanitizer = new HtmlParser();
           
            var rawText = sanitizer.Parse(TextEdit).DocumentElement.TextContent;

            var tree = BcsSyntaxFactory.ParseReaction(rawText);
            var viewModelTreeBuilder = new SyntaxTreeViewModelBuilder();

            if(tree== null) { return;}

            SyntaxToDraw.Clear();

            SyntaxToDraw.Add(viewModelTreeBuilder.Visit(tree));
        }


        private static void LoadBcsFile(string fileName)
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
    }
}
