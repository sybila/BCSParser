using System;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.Syntax.Visitors
{
    public class ParentResolvingVisitor : BcsExpressionNodeVisitor
    {
        protected override void VisitAgent(BcsAtomicAgentNode bcsAtomicAgent)
        {
            ResolveFromParent(bcsAtomicAgent);
        }

        protected override void VisitAgentState(BcsAgentStateNode bcsAgentState)
        {
            ResolveFromParent(bcsAgentState);
        }

        protected override void VisitComplex(BcsComplexNode bcsComplex)
        {
            ResolveFromParent(bcsComplex);
        }

        protected override void VisitComponent(BcsStructuralAgentNode bcsStructuralAgent)
        {
            ResolveFromParent(bcsStructuralAgent);
        }

        protected override void VisitDefault(BcsExpressionNode node)
        {
            throw new InvalidOperationException($"Unsupported node type in visitor: {node.GetType().FullName}.");
        }

        protected override void VisitReactant(BcsReactantNode bcsReactant)
        {
            ResolveFromParent(bcsReactant);
        }

        protected override void VisitReaction(BcsReactionNode bcsReaction)
        {
            ResolveFromParent(bcsReaction);
        }

        protected override void VisitIdentifier(BcsIdentifierNode identifier)
        {
            ResolveFromParent(identifier);
        }

        protected override void VisitAccessor(BcsContentAccessNode node)
        {
            ResolveFromParent(node);
        }

        protected override void VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode)
        {
            ResolveFromParent(bcsVariableExpresssioNode);
        }

        protected override void VisitNamedReference(BcsNamedEntityReferenceNode bcsNamedEntityReferenceNode)
        {
            ResolveFromParent(bcsNamedEntityReferenceNode);
        }

        protected override void VisitNamedEntitySet(BcsNamedEntitySet bcsNamedEntitySet)
        {
            ResolveFromParent(bcsNamedEntitySet);
        }

        private void ResolveFromParent(BcsExpressionNode parentNode)
        {
            foreach (var childNode in parentNode.EnumerateChildNodes())
            {
                childNode.ParentNode = parentNode;
            }
        }
    }
}
