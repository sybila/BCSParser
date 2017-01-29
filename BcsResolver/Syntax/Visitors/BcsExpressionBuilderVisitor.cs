using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.Syntax.Visitors
{
    public abstract class BcsExpressionBuilderVisitor<TResult, TParameter>
    {
        public virtual TResult Visit(BcsExpressionNode node, TParameter parameter = default(TParameter))
        {
            if (node is BcsAtomicAgentNode)
            {
                return VisitAtomicAgent(node as BcsAtomicAgentNode, parameter);
            }
            if (node is BcsAgentStateNode)
            {
                return VisitAgentState(node as BcsAgentStateNode, parameter);
            }
            if (node is BcsStructuralAgentNode)
            {
                return VisitStructuralAgent(node as BcsStructuralAgentNode, parameter);
            }
            if (node is BcsComplexNode)
            {
                return VisitComplex(node as BcsComplexNode, parameter);
            }
            if (node is BcsReactantNode)
            {
                return VisitReactant(node as BcsReactantNode, parameter);
            }
            if (node is BcsReactionNode)
            {
                return VisitReaction(node as BcsReactionNode, parameter);
            }
            if (node is BcsIdentifierNode)
            {
                return VisitIdentifier(node as BcsIdentifierNode, parameter);
            }
            if (node is BcsContentAccessNode)
            {
                return VisitAccessor(node as BcsContentAccessNode, parameter);
            }
            if (node is BcsVariableExpresssioNode)
            {
                return VisitVariableExpression(node as BcsVariableExpresssioNode, parameter);
            }
            if (node is BcsNamedEntityReferenceNode)
            {
                return VisitNamedReference(node as BcsNamedEntityReferenceNode, parameter);
            }
            if (node is BcsNamedEntitySet)
            {
                return VisitNamedEntitySet(node as BcsNamedEntitySet, parameter);
            }
            throw new InvalidOperationException("Unsupported node tipe in visitor.");
        }

        protected abstract TResult VisitNamedEntitySet(BcsNamedEntitySet bcsNamedEntitySet, TParameter parameter);

        protected abstract TResult VisitNamedReference(BcsNamedEntityReferenceNode bcsNamedEntityReferenceNode, TParameter parameter);

        protected abstract TResult VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode, TParameter parameter);

        protected abstract TResult VisitReaction(BcsReactionNode bcsReaction, TParameter parameter);
        protected abstract TResult VisitReactant(BcsReactantNode bcsReactant, TParameter parameter);

        protected abstract TResult VisitComplex(BcsComplexNode bcsComplex, TParameter parameter);

        protected abstract TResult VisitStructuralAgent(BcsStructuralAgentNode bcsStructuralAgent, TParameter parameter);

        protected abstract TResult VisitAgentState(BcsAgentStateNode bcsAgentState, TParameter parameter);

        protected abstract TResult VisitAtomicAgent(BcsAtomicAgentNode bcsAtomicAgent, TParameter parameter);

        protected abstract TResult VisitIdentifier(BcsIdentifierNode identifier, TParameter parameter);

        protected abstract TResult VisitDefault(BcsExpressionNode node);

        protected abstract TResult VisitAccessor(BcsContentAccessNode node, TParameter parameter);
    }
}
