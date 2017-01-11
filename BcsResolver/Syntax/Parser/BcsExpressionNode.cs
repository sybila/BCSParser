using System;
using System.Collections.Generic;
using BcsResolver.Common;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    public abstract class BcsExpressionNode : IBcsTreeNode<BcsExpressionNode>
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        public TextRange ExpressioRange { get; set; }

        public BcsExpressionNode ParentNode { get; set; }

        public List<NodeError> Errors { get; private set; } = new List<NodeError>();

        public virtual IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return new List<BcsExpressionNode>();
        }
    }
}
