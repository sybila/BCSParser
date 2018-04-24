using System;
using BcsResolver.SemanticModel.BoundTree;
using BcsResolver.SemanticModel.SymbolTree;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    public class BcsBoundSymbolFactory : BcsSymbolVisitor<IBcsBoundSymbol, BcsExpressionNode>
    {
        public IBcsBoundSymbol CreateBoundNamedSymbol(BcsNamedSymbol symbol, BcsNamedEntityNode bcsNamedEntityNode, BcsSymbolType expectedType)
        {
            if (symbol == null)
            {
                return new BcsBoundError
                {
                    Syntax = bcsNamedEntityNode,
                    Symbol = new BcsErrorSymbol { Name = bcsNamedEntityNode?.Identifier?.Name ?? "", Error = "No such entity", ExpectedType = expectedType }
                };
            }
            return Visit(symbol, bcsNamedEntityNode);
        }

        protected override IBcsBoundSymbol VisitRule(BcsRuleSymbol bcsRuleSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundReaction()
            {
                Syntax = parameter,
                Symbol = bcsRuleSymbol
            };
        }

        protected override IBcsBoundSymbol VisitError(BcsErrorSymbol bcsErrorSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundError
            {
                Symbol = bcsErrorSymbol,
                Syntax = parameter
            };
        }

        protected override IBcsBoundSymbol VisitLocation(BcsCompartmentSymbol bcsLocationSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundLocation
            {
                Symbol = bcsLocationSymbol,
                Syntax = parameter
            };
        }

        protected override IBcsBoundSymbol VisitComplex(BcsComplexSymbol bcsComplexSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundComplex
            {
                Symbol = bcsComplexSymbol,
                Syntax = parameter
            };
        }

        protected override IBcsBoundSymbol VisitStructuralAgent(BcsStructuralAgentSymbol bcsStructuralAgentSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundStructuralAgent
            {
                Symbol = bcsStructuralAgentSymbol,
                Syntax = parameter
            };
        }

        protected override IBcsBoundSymbol VisitAgentState(BcsStateSymbol bcsStateSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundAgentState
            {
                Symbol = bcsStateSymbol,
                Syntax = parameter
            };
        }

        protected override IBcsBoundSymbol VisitAtomicAgent(BcsAtomicAgentSymbol bcsAtomicAgentSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundAtomicAgent
            {
                Symbol = bcsAtomicAgentSymbol,
                Syntax = parameter
            };
        }

        protected override IBcsBoundSymbol VisitVariable(BcsVariableSymbol bcsVariableSymbol, BcsExpressionNode parameter)
        {
            return new BcsBoundVariableExpression
            { 
                Symbol = bcsVariableSymbol,
                Syntax = parameter
            };
        }

        protected override IBcsBoundSymbol VisitDefault(BcsSymbol symbol, BcsExpressionNode parameter)
        {
            throw new NotImplementedException("All visits sould be overloaded");
        }
    }
}