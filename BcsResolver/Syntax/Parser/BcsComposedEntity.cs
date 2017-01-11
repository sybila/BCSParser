using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    public abstract class BcsComposedEntity : BcsNamedEntityNode
    {
        public List<BcsExpressionNode> Parts { get; } = new List<BcsExpressionNode>();
        public List<TextRange> Separators { get; } = new List<TextRange>();
        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes().Concat(Parts);
        }
    }
}