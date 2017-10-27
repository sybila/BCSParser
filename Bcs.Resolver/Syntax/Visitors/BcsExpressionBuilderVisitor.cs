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
            throw new InvalidOperationException("Unsupported node type in visitor.");
        }

        protected virtual TResult VisitNamedEntitySet(BcsNamedEntitySet bcsNamedEntitySet, TParameter parameter) { return VisitDefault(bcsNamedEntitySet, parameter); }
        protected virtual TResult VisitNamedReference(BcsNamedEntityReferenceNode bcsNamedEntityReferenceNode, TParameter parameter) { return VisitDefault(bcsNamedEntityReferenceNode, parameter); }
        protected virtual TResult VisitVariableExpression(BcsVariableExpresssioNode bcsVariableExpresssioNode, TParameter parameter) { return VisitDefault(bcsVariableExpresssioNode, parameter); }
        protected virtual TResult VisitReaction(BcsReactionNode bcsReaction, TParameter parameter) { return VisitDefault(bcsReaction, parameter); }
        protected virtual TResult VisitReactant(BcsReactantNode bcsReactant, TParameter parameter) { return VisitDefault(bcsReactant, parameter); }
        protected virtual TResult VisitComplex(BcsComplexNode bcsComplex, TParameter parameter) { return VisitDefault(bcsComplex, parameter); }
        protected virtual TResult VisitStructuralAgent(BcsStructuralAgentNode bcsStructuralAgent, TParameter parameter) { return VisitDefault(bcsStructuralAgent, parameter); }
        protected virtual TResult VisitAgentState(BcsAgentStateNode bcsAgentState, TParameter parameter) { return VisitDefault(bcsAgentState, parameter); }
        protected virtual TResult VisitAtomicAgent(BcsAtomicAgentNode bcsAtomicAgent, TParameter parameter) { return VisitDefault(bcsAtomicAgent, parameter); }
        protected virtual TResult VisitIdentifier(BcsIdentifierNode bcsIdentifier, TParameter parameter) { return VisitDefault(bcsIdentifier, parameter); }
        protected virtual TResult VisitAccessor(BcsContentAccessNode node, TParameter parameter) { return VisitDefault(node, parameter); }
        protected abstract TResult VisitDefault(BcsExpressionNode node, TParameter parameter);
    }
}
