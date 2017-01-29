using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    [DebuggerDisplay("[BAS: {ToString()}]")]
    public class BcsBoundAgentState : BcsBoundSymbol<BcsStateSymbol> { }

    [DebuggerDisplay("[BER: {ToString()}]")]
    public class BcsBoundError : BcsBoundSymbol<BcsErrorSymbol> { }

    [DebuggerDisplay("[BL: {ToString()}]")]
    public class BcsBoundLocation : BcsBoundSymbol<BcsLocationSymbol>
    {
        public IBcsBoundSymbol Content { get; set; }
    }

    public abstract class BcsBoundSymbol<TSymbol> : IBcsBoundSymbol
        where TSymbol : BcsSymbol
    {
        public BcsExpressionNode Syntax { get; set; }
        public TSymbol Symbol { get; set; }
        BcsSymbol IBcsBoundSymbol.Symbol => Symbol;
        public List<SemanticError> Errors { get; set; } = new List<SemanticError>();

        public override string ToString() => $"{Syntax.ToDisplayString()} --> {Symbol.ToDisplayString()}";
    }

    public interface IBcsBoundSymbol
    {
        BcsExpressionNode Syntax { get; set; }
        BcsSymbol Symbol { get; }
        List<SemanticError> Errors { get; set; }
    }
}
