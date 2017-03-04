using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.SemanticModel.SymbolTree;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel.BoundTree
{
    [DebuggerDisplay("[BAS: {ToString()}]")]
    public class BcsBoundAgentState : BcsBoundSymbol<BcsStateSymbol>
    {
        public override IEnumerable<IBcsBoundSymbol> GetChildren() => new List<IBcsBoundSymbol>();
    }

    [DebuggerDisplay("[BER: {ToString()}]")]
    public class BcsBoundError : BcsBoundSymbol<BcsErrorSymbol>
    {
        public override IEnumerable<IBcsBoundSymbol> GetChildren() => new List<IBcsBoundSymbol>();
    }

    [DebuggerDisplay("[BL: {ToString()}]")]
    public class BcsBoundLocation : BcsBoundSymbol<BcsLocationSymbol>
    {
        public IBcsBoundSymbol Content { get; set; }

        public override IEnumerable<IBcsBoundSymbol> GetChildren()
        {
            return Content == null ? Enumerable.Empty<IBcsBoundSymbol>() : new[] {Content};
        }
    }

    [DebuggerDisplay("[BVE: {ToString()}]")]
    public class BcsBoundVariableExpression : BcsBoundSymbol<BcsVariableSymbol>
    {
        public IBcsBoundSymbol Target { get; set; }

        public override IEnumerable<IBcsBoundSymbol> GetChildren()
        {
            return Target == null ? Enumerable.Empty<IBcsBoundSymbol>() : new[] { Target };
        }

    }


    public abstract class BcsBoundSymbol<TSymbol> : IBcsBoundSymbol
        where TSymbol : BcsSymbol
    {
        public BcsExpressionNode Syntax { get; set; }
        public TSymbol Symbol { get; set; }
        BcsSymbol IBcsBoundSymbol.Symbol => Symbol;

        public override string ToString() => $"{Syntax.ToDisplayString()} --> {Symbol.ToDisplayString()}";
        public abstract IEnumerable<IBcsBoundSymbol> GetChildren();
    }

    public interface IBcsBoundSymbol
    {
        BcsExpressionNode Syntax { get; set; }
        BcsSymbol Symbol { get; }
        IEnumerable<IBcsBoundSymbol> GetChildren();
    }
}
