using BcsResolver.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Parser;
using System.Collections.ObjectModel;

namespace BcsExplorerDemo
{
    public class MenuItemVisitor : BcsExpressionNodeVisitor 
    {
        public IDictionary<Guid, MenuItem> NodeCache { get; set; } = new Dictionary<Guid, MenuItem>();

        public MenuItem Root { get; set; }

        protected override void VisitAgent(BcsAtomicAgentNode bcsAtomicAgent)
        {
            var item = new MenuItem() { Title = $"Agent: {bcsAtomicAgent.Name}{{{bcsAtomicAgent?.CurrentState?.Name ?? ""}}}" };

            AddToTree(bcsAtomicAgent, item);
        }

        protected override void VisitAgentState(BcsAgentStateNode bcsAgentState)
        {
            bool isCurrentState = bcsAgentState.ParentNode != null && (bcsAgentState.ParentNode as BcsAtomicAgentNode).CurrentState.UniqueId == bcsAgentState.UniqueId;

            var item = new MenuItem() { Title = $"State{(isCurrentState ? " - current" : string.Empty)}: {bcsAgentState.Name}" };

            AddToTree(bcsAgentState, item);
        }

        protected override void VisitComplex(BcsComplexNode bcsComplex)
        {
            var item = new MenuItem() { Title = $"Complex: {GetComplexFullName(bcsComplex)}" };

            AddToTree(bcsComplex, item);
        }

        protected override void VisitComponent(BcsComponentNode bcsComponent)
        {
            var item = new MenuItem() { Title = $"Component: {bcsComponent.Name}::{bcsComponent?.Location?.Name ?? string.Empty}" };

            AddToTree(bcsComponent, item);
        }

        protected override void VisitDefault(BcsExpressionNode node)
        {
            var item = new MenuItem() { Title = $"Unknown" };

            AddToTree(node, item);
        }

        protected override void VisitReactant(BcsReactantNode bcsReactant)
        {
            var item = new MenuItem() { Title = $"Reactant: {bcsReactant.Coeficient} {GetComplexFullName(bcsReactant.Complex)}" };

            AddToTree(bcsReactant, item);
        }

        protected override void VisitReaction(BcsReactionNode bcsReaction)
        {
            var item = new MenuItem()
            {
                Drawable = true,
                SyntaxTreeEntityId = bcsReaction.UniqueId,
                Title = $"Reaction: L={bcsReaction.LeftSideReactants.Count} {bcsReaction.ReactionDirection} R={bcsReaction.RightSideReactants.Count}"
            };

            NodeCache.Add(bcsReaction.UniqueId, item);
            Root = item;
        }

        protected override void VisitLocation(BcsLocationNode locationNode)
        {
            var item = new MenuItem() { Title = $"Location: {locationNode.Name}" };
            AddToTree(locationNode, item);
        }

        private void AddToTree(BcsExpressionNode node, MenuItem item)
        {
            //ISSUE: make dedicated function for errors
            foreach (var error in node.Errors)
            {
                item.NodeErrors.Add(error.Message);
            }

            var parentItem = NodeCache[node.ParentNode.UniqueId];

            parentItem.Items.Add(item);
            NodeCache.Add(node.UniqueId, item);
        }

        private static string GetComplexFullName(BcsComplexNode complex)
        {
            if (complex == null)
            {
                return "[]";
            }

            var componentNames = complex.Components
                .Select(c =>
                {
                    return (c is BcsLocationNode) ? ((c as BcsLocationNode).Resident?.Name ?? "<error>") : c.Name;
                });

            return $"[{string.Join(".", componentNames)}]";
        }
    }
}
