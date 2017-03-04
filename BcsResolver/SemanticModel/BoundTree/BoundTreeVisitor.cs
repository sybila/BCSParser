using System;
using System.Security.Policy;
using BcsResolver.Extensions;

namespace BcsResolver.SemanticModel.BoundTree
{
    public abstract class BoundTreeVisitor
    {
        public virtual void Visit(IBcsBoundSymbol boundSymbol)
        {
            if (boundSymbol.Is<BcsBoundLocation>())
            {
                VisitBoundLocation(boundSymbol.CastTo<BcsBoundLocation>());
            }
            else if (boundSymbol.Is<BcsBoundAgentState>())
            {
                VisitBoundAgentState(boundSymbol.CastTo<BcsBoundAgentState>());
            }
            else if (boundSymbol.Is<BcsBoundAtomicAgent>())
            {
                VisitBoundAtomicAgent(boundSymbol.CastTo<BcsBoundAtomicAgent>());
            }
            else if (boundSymbol.Is<BcsBoundStructuralAgent>())
            {
                VisitBoundStructuralAgent(boundSymbol.CastTo<BcsBoundStructuralAgent>());
            }
            
            else if (boundSymbol.Is<BcsBoundComplex>())
            {
                VisitBoundComplex(boundSymbol.CastTo<BcsBoundComplex>());
            }
            else if (boundSymbol.Is<BcsBoundReaction>())
            {
                VisitBoundReaction(boundSymbol.CastTo<BcsBoundReaction>());
            }
            else if (boundSymbol.Is<BcsBoundVariableExpression>())
            {
                VisitBoundVariableExpression(boundSymbol.CastTo<BcsBoundVariableExpression>());
            }
            else if (boundSymbol.Is<BcsBoundError>())
            {
                VisitBoundError(boundSymbol.CastTo<BcsBoundError>());
            }
            else
            {
                throw new InvalidOperationException("Node not supported by visitor");
            }

            foreach (var child in boundSymbol.GetChildren())
            {
                Visit(child);
            }
        }

        protected virtual void VisitBoundVariableExpression(BcsBoundVariableExpression castTo) { VisitDefault(castTo); }

        protected virtual void VisitBoundError(BcsBoundError castTo) { VisitDefault(castTo);}

        protected virtual void VisitBoundReaction(BcsBoundReaction castTo) { VisitDefault(castTo); }

        protected virtual void VisitBoundComplex(BcsBoundComplex castTo) { VisitDefault(castTo); }

        protected virtual void VisitBoundAtomicAgent(BcsBoundAtomicAgent castTo) { VisitDefault(castTo); }

        protected virtual void VisitBoundStructuralAgent(BcsBoundStructuralAgent castTo) { VisitDefault(castTo); }

        protected virtual void VisitBoundAgentState(BcsBoundAgentState castTo) { VisitDefault(castTo); }

        protected virtual void VisitBoundLocation(BcsBoundLocation castTo) { VisitDefault(castTo); }

        public abstract void VisitDefault(IBcsBoundSymbol boundSymbol);
    }
}
