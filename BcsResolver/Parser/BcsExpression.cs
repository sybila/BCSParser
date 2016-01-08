using BcsResolver.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Parser
{
    public abstract class BcsExpressionNode
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
