using BcsResolver.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Visitors;

namespace BcsExplorerDemo
{
    public class MenuItemVisitor : BcsExpressionNodeVisitor
    {
        public IDictionary<object, MenuItem> NodeCache { get; set; } = new Dictionary<object, MenuItem>();

        public MenuItem Root { get; set; }

        protected override void VisitAgent(BcsAtomicAgentNode bcsAtomicAgent)
        {
            AddNodeToTree(bcsAtomicAgent);
        }

        protected override void VisitAgentState(BcsAgentStateNode bcsAgentState)
        {
            AddNodeToTree(bcsAgentState);
        }

        protected override void VisitComplex(BcsComplexNode bcsComplex)
        {
            AddNodeToTree(bcsComplex);
        }

        protected override void VisitComponent(BcsStructuralAgentNode bcsStructuralAgentNode)
        {
            AddNodeToTree(bcsStructuralAgentNode);
        }

        protected override void VisitDefault(BcsExpressionNode node)
        {
            AddNodeToTree(node);
        }

        protected override void VisitReactant(BcsReactantNode bcsReactant)
        {
            AddNodeToTree(bcsReactant);
        }

        protected override void VisitReaction(BcsReactionNode bcsReaction)
        {
            var item = new MenuItem()
            {
                Drawable = true,
                Title = bcsReaction.ToDisplayString(),
                SyntaxTreeEntityId = bcsReaction.UniqueId
            };

            NodeCache.Add(bcsReaction, item);
            Root = item;
        }

        protected override void VisitNamedEntitySet(BcsNamedEntitySet bcsNamedEntitySet)
        {
            AddNodeToTree(bcsNamedEntitySet);
        }

        protected override void VisitNamedReference(BcsNamedEntityReferenceNode bcsNamedEntityReferenceNode)
        {
            AddNodeToTree(bcsNamedEntityReferenceNode);
        }

        protected override void VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode)
        {
            AddNodeToTree(bcsVariableExpresssioNode);
        }

        protected override void VisitIdentifier(BcsIdentifierNode identifier)
        {
            AddNodeToTree(identifier);
        }

        protected override void VisitAccessor(BcsContentAccessNode node)
        {
            AddNodeToTree(node);
        }

        private void AddNodeToTree<TNode>(TNode syntaxNode)
          where TNode : BcsExpressionNode
        {
            var item = new MenuItem() { Title = $"{typeof(TNode).Name}: <{syntaxNode.ToDisplayString()}>" };

            AddToTree(syntaxNode, item);
        }

        private void AddToTree(BcsExpressionNode node, MenuItem item)
        {
            //ISSUE: make dedicated function for errors
            foreach (var error in node.Errors)
            {
                item.NodeErrors.Add(error.Message);
            }

            var parentItem = NodeCache[node.ParentNode];

            parentItem.Items.Add(item);
            NodeCache.Add(node, item);
        }
    }
}
