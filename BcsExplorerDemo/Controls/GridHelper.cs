using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.BoundTree;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Visitors;

namespace BcsExplorerDemo.Controls
{
    public class GridHelper
    {
        public Grid CreateBoundSymbol(IBcsBoundSymbol boundSymbol)
        {
            if (boundSymbol == null)
            {
                return new Grid()
                {
                    Background = Brushes.Red,
                    Margin = new Thickness(5, 2, 2, 0)
                };
            }
            if (boundSymbol is IBcsComposedBoundSymbol)
            {
                return CreateBoundComposedSymbol(boundSymbol.CastTo<IBcsComposedBoundSymbol>());
            }
            if (boundSymbol is BcsBoundLocation)
            {
                return CreateBoundLocation(boundSymbol.CastTo<BcsBoundLocation>());
            }
            return CreateBoundSymbolOnly(boundSymbol);
        }

        private Grid CreateBoundLocation(BcsBoundLocation location)
        {
            var grid = new Grid()
            {
                Background = Brushes.Aquamarine,
                Margin = new Thickness(15,5,5,0)
            };
            SetBoundSymbolPropertirs(location, grid);
            AddContentLabel(grid);
            AddRowControlToGrid(grid, CreateBoundSymbol(location.Content));
            return grid;
        }

        private static void AddContentLabel(Grid grid)
        {
            AddRowControlToGrid(grid, new Label
            {
                FontSize = 12,
                Background = Brushes.Transparent,
                Content = "Content:",
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            });
        }

        public Grid CreateBoundSymbolOnly(IBcsBoundSymbol boundSymbol)
        {
            var grid = new Grid()
            {
                Background = Brushes.Aqua,
                Margin = new Thickness(5, 2, 2, 0)
            };
            SetBoundSymbolPropertirs(boundSymbol, grid);
            return grid;
        }

       

        public Grid CreateBoundComposedSymbol(IBcsComposedBoundSymbol composedBoundSymbol)
        {
            var grid = new Grid()
            {
                Background = Brushes.LawnGreen,
                Margin = new Thickness(5, 2, 2, 0)
            };
            SetBoundSymbolPropertirs(composedBoundSymbol, grid);
            AddContentLabel(grid);
            foreach (var pair in composedBoundSymbol.StatedContent.Values.SelectMany(v=> v))
            {
                AddRowControlToGrid(grid, CreateBoundSymbol(pair));
            }
            return grid;
        }


        private static void AddColumnControlToGrid(Grid grid, UIElement control, int row = 0)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new System.Windows.GridLength(1, GridUnitType.Star) });
            Grid.SetColumn(control, grid.ColumnDefinitions.Count - 1);
            Grid.SetRow(control, row);
            grid.Children.Add(control);
        }

        private static void AddRowControlToGrid(Grid grid, UIElement control)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(1, GridUnitType.Star) });
            Grid.SetRow(control, grid.RowDefinitions.Count - 1);
            grid.Children.Add(control);
        }

        private static void AddReactionSymbolColumn(Grid grid, string label)
        {
            AddRowControlToGrid(grid, new Label
            {
                FontSize = 12,
                Background = Brushes.LawnGreen,
                Content = label,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }

        private static void SetBoundSymbolPropertirs(IBcsBoundSymbol boundSymbol, Grid grid)
        {
            AddRowControlToGrid(grid, new Label
            {
                Content = $"Syntax: {boundSymbol.Syntax.ToDisplayString()}",
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 12,
                Background = Brushes.MediumPurple,
            });
            AddRowControlToGrid(grid, new Label
            {
                Content = $"Symbol: {boundSymbol.Symbol?.ToDisplayString()?? "none"}",
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 12,
                Background = Brushes.DeepSkyBlue,
            });
        }
    }
}
