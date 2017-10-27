using System.Collections.Generic;
using System.Diagnostics;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[S: {Identifier}]")]
    public abstract class BcsNamedEntityNode : BcsExpressionNode
    {
        public BcsIdentifierNode Identifier { get; set; }

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            if (Identifier == null)
            {
                return new BcsExpressionNode[] {};
            }
            return new BcsExpressionNode[] { Identifier };
        }
    }
}