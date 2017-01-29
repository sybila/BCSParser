using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Visitors;

namespace BcsExplorerDemo.Controls
{
    public class GridHelper : BcsExpressionBuilderVisitor<Grid, object>
    {



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
            AddColumnControlToGrid(grid, new Label
            {
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Background = Brushes.LawnGreen,
                Content = label,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });
        }

        private static void AddEmptyRow(Grid grid)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new System.Windows.GridLength(1, GridUnitType.Star) });
        }

        protected override Grid VisitNamedEntitySet(BcsNamedEntitySet bcsNamedEntitySet, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitNamedReference(BcsNamedEntityReferenceNode bcsNamedEntityReferenceNode, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitReaction(BcsReactionNode bcsReaction, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitReactant(BcsReactantNode bcsReactant, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitComplex(BcsComplexNode bcsComplex, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitStructuralAgent(BcsStructuralAgentNode bcsStructuralAgent, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitAgentState(BcsAgentStateNode bcsAgentState, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitAtomicAgent(BcsAtomicAgentNode bcsAtomicAgent, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitIdentifier(BcsIdentifierNode identifier, object parameter)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitDefault(BcsExpressionNode node)
        {
            throw new NotImplementedException();
        }

        protected override Grid VisitAccessor(BcsContentAccessNode node, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
