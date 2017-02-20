using System.Collections.Generic;
using System.Linq;
using BcsResolver.Extensions;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    public class BcsComposedSymbolMerger
    {
        public Dictionary<BcsExpressionNode, List<SemanticError>> Errors { get; } = new Dictionary<BcsExpressionNode, List<SemanticError>>();

        public IBcsComposedBoundSymbol MergeFromAccessor(IBcsComposedBoundSymbol parentBoundSymbol, IBcsBoundSymbol childBoundSymbol)
        {
            if (parentBoundSymbol is BcsBoundStructuralAgent && childBoundSymbol is BcsBoundAtomicAgent)
            {
                var parentStructuralAgent = parentBoundSymbol.As<BcsBoundStructuralAgent>();
                var childAtomicAgent = childBoundSymbol.As<BcsBoundAtomicAgent>();

                var candidateMatches =
                    parentStructuralAgent.StatedContent.GetValueOrDefault(childAtomicAgent.Symbol.Name);

                if (candidateMatches.Count > 0)
                {
                    AddError(parentStructuralAgent.Syntax, "More than one Atomic agent of the same name.", SemanticErrorSeverity.Error);
                }
                //TODO
            }
            return null;
        }

        private void AddError(BcsExpressionNode identifier, string message, SemanticErrorSeverity severity)
        {
            if (!Errors.ContainsKey(identifier))
            {
                Errors[identifier] = new List<SemanticError>();
            }
            Errors[identifier].Add(new SemanticError(message, severity));
        }
    }
}