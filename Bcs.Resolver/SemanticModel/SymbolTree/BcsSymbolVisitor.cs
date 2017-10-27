using System;
using BcsResolver.SemanticModel.SymbolTree;

namespace BcsResolver.SemanticModel.Tree
{
    public abstract class BcsSymbolVisitor<TResult, TParameter>
    {
        public virtual TResult Visit(BcsSymbol symbol, TParameter parameter)
        {
            if (symbol is BcsAtomicAgentSymbol)
            {
                return VisitAtomicAgent(symbol as BcsAtomicAgentSymbol, parameter);
            }
            if (symbol is BcsStateSymbol)
            {
                return VisitAgentState(symbol as BcsStateSymbol, parameter);
            }
            if (symbol is BcsStructuralAgentSymbol)
            {
                return VisitStructuralAgent(symbol as BcsStructuralAgentSymbol, parameter);
            }
            if (symbol is BcsComplexSymbol)
            {
                return VisitComplex(symbol as BcsComplexSymbol, parameter);
            }
            if (symbol is BcsLocationSymbol)
            {
                return VisitLocation(symbol as BcsLocationSymbol, parameter);
            }
            if (symbol is BcsRuleSymbol)
            {
                return VisitRule(symbol as BcsRuleSymbol, parameter);
            }
            if (symbol is BcsVariableSymbol)
            {
                return VisitVariable(symbol as BcsVariableSymbol, parameter);
            }
            if (symbol is BcsErrorSymbol)
            {
                return VisitError(symbol as BcsErrorSymbol, parameter);
            }
            throw new InvalidOperationException("Unsupported node type in visitor.");
        }

        protected virtual TResult VisitVariable(BcsVariableSymbol bcsVariableSymbol, TParameter parameter)
        {
            return VisitDefault(bcsVariableSymbol, parameter);
        }

        protected virtual TResult VisitRule(BcsRuleSymbol bcsRuleSymbol, TParameter parameter)
        {
            return VisitDefault(bcsRuleSymbol, parameter);
        }

        protected virtual TResult VisitError(BcsErrorSymbol bcsErrorSymbol, TParameter parameter)
        {
            return VisitDefault(bcsErrorSymbol, parameter);
        }

        protected virtual TResult VisitLocation(BcsLocationSymbol bcsLocationSymbol, TParameter parameter)
        {
            return VisitDefault(bcsLocationSymbol, parameter);
        }

        protected virtual TResult VisitComplex(BcsComplexSymbol bcsComplexSymbol, TParameter parameter)
        {
            return VisitDefault(bcsComplexSymbol, parameter);
        }

        protected virtual TResult VisitStructuralAgent(BcsStructuralAgentSymbol bcsStructuralAgentSymbol, TParameter parameter)
        {
            return VisitDefault(bcsStructuralAgentSymbol, parameter);
        }

        protected virtual TResult VisitAgentState(BcsStateSymbol bcsStateSymbol, TParameter parameter)
        {
            return VisitDefault(bcsStateSymbol, parameter);
        }

        protected virtual TResult VisitAtomicAgent(BcsAtomicAgentSymbol bcsAtomicAgentSymbol, TParameter parameter)
        {
            return VisitDefault(bcsAtomicAgentSymbol, parameter);
        }

        protected abstract TResult VisitDefault(BcsSymbol symbol, TParameter parameter);
    }
}
