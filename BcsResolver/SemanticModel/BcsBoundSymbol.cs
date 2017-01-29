using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    public class BcsBoundAgentState : BcsBoundSymbol<BcsStateSymbol> { }

    public abstract class BcsBoundSymbol<TSymbol> : IBcsBoundSymbol
        where TSymbol : BcsSymbol
    {
        public BcsExpressionNode Syntax { get; set; }
        public TSymbol Symbol { get; set; }
        BcsSymbol IBcsBoundSymbol.Symbol => Symbol;
        public List<SemanticError> Errors { get; set; } = new List<SemanticError>();
    }

    public interface IBcsBoundSymbol
    {
        BcsExpressionNode Syntax { get; set; }
        BcsSymbol Symbol { get; }
        List<SemanticError> Errors { get; set; } 
    }
}
