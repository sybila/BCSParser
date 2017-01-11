using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    public class BcsBoundSymbol<TSymbol>
        where TSymbol : BcsSymbol
    {
        public BcsExpressionNode Syntax { get; set; }
        public TSymbol Symbol { get; set; }
        public List<SemanticError> Errors { get; set; } = new List<SemanticError>();
    }
}
