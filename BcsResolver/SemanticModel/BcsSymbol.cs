using System.Collections.Generic;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    public abstract class BcsSymbol
    {
        public BcsSymbolType Type { get; set; } = BcsSymbolType.Unknown;
        public BcsExpressionNode Syntax { get; set; }
        public List<SemanticError> Errors { get; } = new List<SemanticError>();
        public BcsSymbol Parent { get; set; }
    }
}
