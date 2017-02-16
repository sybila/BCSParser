using BcsResolver.File;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BcsExplorerDemo.Controls;
using BcsResolver.SemanticModel;
using BcsResolver.Syntax;
using BcsResolver.Syntax.Parser;

namespace BcsExplorerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<Guid,BcsReactionNode> reactions;
        private BcsDefinitionFile document;
        private BcsWorkspace workspace;

        public ObservableCollection<string> StateNames;

        public MainWindow()
        {
            InitializeComponent();

            LoadBcsFile("yamada.txt");

            reactionTree.Items.Add(BuildTreeView());

            workspace = new BcsWorkspace(new BcsFileMetadataProvider(document));
            workspace.CreateSemanticModel();
        }

        public void LoadBcsFile(string fileName)
        {
            using (var bcsHandler = new BcsDefinitionFileReader())
            {
                document = bcsHandler.ReadFile("yamada.txt");
            }

            reactions = document.Rules
                .Select(r => BcsSyntaxFactory.ParseReaction(r.Equation))
                .Cast<BcsReactionNode>()
                .ToDictionary(key => key.UniqueId);

            StateNames = new ObservableCollection<string>();     
        }

        private MenuItem BuildTreeView()
        {
            MenuItem root = new MenuItem() { Title = "Reactions" };

            foreach (var reaction in reactions)
            {
                try
                {
                    var visitor = new MenuItemVisitor();
                    visitor.Visit(reaction.Value);
                    root.Items.Add(visitor.Root);
                }
                catch (Exception)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }
                }
            }

            return root;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;
            var id = (Guid) senderButton?.Tag;

            DrawReactionInCanvas(id);
        }

        private void DrawReactionInCanvas(Guid id)
        {
            MainCanvas.Children.Clear();

            var gridHelper = new GridHelper();
            var reactionTree = reactions[id];
            var binder = new SemanticAnalisisVisitor(workspace, new BcsBoundSymbolFactory());

            var bound = binder.Visit(reactionTree);

            var control = gridHelper.CreateBoundSymbol(bound);

            Grid.SetColumn(control, 1);
            MainCanvas.Children.Add(control);
        }
    }
    public class MenuItem
    {
        public bool Drawable { get; set; } = false;
        public Visibility DrawButtonVisibility => Drawable ? Visibility.Visible : Visibility.Hidden;
        public Guid SyntaxTreeEntityId { get; set; } = Guid.Empty;
        public string Title { get; set; }

        public ObservableCollection<string> NodeErrors { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<MenuItem> Items { get; private set; } = new ObservableCollection<MenuItem>();
    }
}
