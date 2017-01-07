using System.Collections.Generic;
using System.Diagnostics;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[S: {Name}]")]
    public abstract class BcsNamedEntitySyntax : BcsExpressionNode
    {
        public BcsIdentifierNode Name { get; set; }

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            if (Name == null)
            {
                return new BcsExpressionNode[] {};
            }
            return new BcsExpressionNode[] { Name };
        }
    }
}