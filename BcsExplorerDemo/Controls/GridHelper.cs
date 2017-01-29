//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using BcsResolver.Syntax.Parser;

//namespace BcsExplorerDemo.Controls
//{
//    public class GridHelper
//    {
//        public static Grid CreateReactionGrid(BcsReactionNode reaction)
//        {
//            var grid = new Grid
//            {
//                Background = Brushes.Brown
//            };

//            foreach (var reactant in reaction.LeftSideReactants)
//            {
//                AddColumnControlToGrid(grid, CreateReactantGrid(reactant));

//                if (reaction.LeftSideReactants.IndexOf(reactant) != reaction.LeftSideReactants.Count - 1)
//                {
//                    AddReactionSymbolColumn(grid, "+");
//                }
//            }

//            string reactionMark = reaction.ReactionDirection == ReactionDirectionType.Both ? "<=>" : (reaction.ReactionDirection == ReactionDirectionType.Left ? "<=" : "=>");
//            AddReactionSymbolColumn(grid, reactionMark);

//            foreach (var reactant in reaction.RightSideReactants)
//            {
//                AddColumnControlToGrid(grid, CreateReactantGrid(reactant));

//                if (reaction.RightSideReactants.IndexOf(reactant) != reaction.RightSideReactants.Count - 1)
//                {
//                    AddReactionSymbolColumn(grid, "+");
//                }
//            }

//            return grid;
//        }

//        public static Grid CreateReactantGrid(BcsReactantNode reactant)
//        {
//            var grid = new Grid
//            {
//                Background = Brushes.AliceBlue
//            };

//            AddColumnControlToGrid(grid, new Label
//            {
//                Content = reactant.Coeficient,
//                VerticalContentAlignment = VerticalAlignment.Center,
//                HorizontalAlignment = HorizontalAlignment.Center,
//                FontSize = 20,
//                FontWeight = FontWeights.Bold,
//                Background = Brushes.LawnGreen,
//            });

//            AddColumnControlToGrid(grid, CreateComplexGrid(reactant.Complex));

//            return grid;
//        }

//        public static Grid CreateComplexGrid(BcsComplexNode complex)
//        {
//            var grid = new Grid();

//            int componentRowIndex = 0;

//            if(complex.Components.Count > 1)
//            {
//                AddRowControlToGrid(grid, new Label { Content = "Complex:" });
//                AddEmptyRow(grid);
//                componentRowIndex = 1;
//            }

//            foreach (var component in complex.Components)
//            {
//                AddColumnControlToGrid(grid, CreateEntityGrid(component), componentRowIndex);
//            }

//            return grid;
//        }

//        public static Grid CreateEntityGrid(BcsIdentifierNode identifier)
//        {
//            var grid = new Grid
//            {
//                Background = Brushes.GreenYellow,
//                Margin = new Thickness(10, 10, 10, 10)
//            };

//            if (identifier is BcsLocationNode)
//            {
//                var location = identifier as BcsLocationNode;
//                AddRowControlToGrid(grid, new Label { Content = $"Location: {location.Name}" });
//                AddRowControlToGrid(grid, CreateEntityGrid(location.Resident));
//            }
//            else if (identifier is BcsComponentNode)
//            {
//                int componentRowIndex = 1;
//                var component = identifier as BcsComponentNode;
//                AddRowControlToGrid(grid, new Label { Content = $"Component: {component.Name}" });

//                AddEmptyRow(grid);

//                if (component.AtomicAgents.Any())
//                {
//                    grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(1, GridUnitType.Star) });
//                    componentRowIndex++;
//                }

//                foreach (var agent in component.AtomicAgents)
//                {
//                    AddColumnControlToGrid(grid, CreateAgentGrid(agent), 1);
//                }

//                foreach (var childComponent in component.SubComponents)
//                {
//                    AddColumnControlToGrid(grid, CreateEntityGrid(childComponent), componentRowIndex);
//                }
//            }

//            return grid;
//        }

//        public static UIElement CreateAgentGrid(BcsAtomicAgentNode agent)
//        {
//            var gridBorder = new Border
//            {
//                Margin = new Thickness(5),
//                BorderThickness = new Thickness(3),
//                CornerRadius = new CornerRadius(3),
//                BorderBrush = Brushes.LightBlue
//            };

//            var grid = new Grid
//            {
//                Background = Brushes.LightBlue,
//            };

//            gridBorder.Child = grid;

//            AddRowControlToGrid(grid, new Label { Content = $"Agent: {agent.Name}" });
//            AddRowControlToGrid(grid, new Label { Content = $"Required state: {agent.CurrentState?.Name ?? "-"}" });

//            string possibleStatesText = $"Possible states: {string.Join(", ", agent.AllStates.Select(agentState => agentState.Name ?? string.Empty))}";
//            AddRowControlToGrid(grid, new Label { Content = possibleStatesText });

//            return gridBorder;
//        }

//        private static void AddColumnControlToGrid(Grid grid, UIElement control, int row = 0)
//        {
//            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new System.Windows.GridLength(1, GridUnitType.Star) });
//            Grid.SetColumn(control, grid.ColumnDefinitions.Count - 1);
//            Grid.SetRow(control, row);
//            grid.Children.Add(control);
//        }

//        private static void AddRowControlToGrid(Grid grid, UIElement control)
//        {
//            grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(1, GridUnitType.Star) });
//            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
//            grid.Children.Add(control);
//        }

//        private static void AddReactionSymbolColumn(Grid grid, string label)
//        {
//            AddColumnControlToGrid(grid, new Label {
//                FontSize = 20,
//                FontWeight = FontWeights.Bold,
//                Background = Brushes.LawnGreen,
//                Content = label,
//                VerticalContentAlignment = VerticalAlignment.Center,
//                HorizontalAlignment = HorizontalAlignment.Center
//            });
//        }

//        private static void AddEmptyRow(Grid grid)
//        {
//            grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(1, GridUnitType.Star) });
//        }
//    }
//}
