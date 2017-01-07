using BcsExplorerDemo.Controls;
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
using BcsResolver.Syntax.Parser;

namespace BcsExplorerDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<Guid, BcsReactionNode> _reactions;
        private BcsDefinitionFile _document;

        public ObservableCollection<string> StateNames;

        public MainWindow()
        {
            InitializeComponent();

            LoadBcsFile("yamada.txt");

            reactionTree.Items.Add(BuildTreeView());

            var firstReaction = _reactions.Values.First();

            DrawReactionInCanvas(firstReaction);
        }

        private void DrawReactionInCanvas(BcsReactionNode firstReaction)
        {
            MainCanvas.Children.Clear();
            var grid = GridHelper.CreateReactionGrid(firstReaction);
            Grid.SetColumn(grid, 1);
            MainCanvas.Children.Add(grid);
        }

        public void LoadBcsFile(string fileName)
        {
            BcsDefinitionFile document;

            using (var bcsHandler = new BcsWorkspace())
            {
                bcsHandler.ProcessDefinitionFile("yamada.txt");
                document = bcsHandler.DefinitionFile;
            }

            _reactions = document.Rules.Select(r => r.Equation.ExpressionNode).ToDictionary(reaction => reaction.UniqueId);
            _document = document;

            StateNames = new ObservableCollection<string>();

            foreach(var state in document.States)
            {
                StateNames.Add(state.Name);
            }
        }

        private MenuItem BuildTreeView()
        {
            MenuItem root = new MenuItem() { Title = "Reactions" };

            foreach (var reaction in _reactions.Values)
            {
                try
                {
                    var visitor = new MenuItemVisitor();
                    visitor.Visit(reaction);
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
            var id = (Guid) senderButton.Tag;

            if(_reactions.ContainsKey(id))
            {
                DrawReactionInCanvas(_reactions[id]);
            }
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
