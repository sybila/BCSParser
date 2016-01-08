using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BcsResolver.Parser
{
    [DebuggerDisplay("[{Name}::{Location?.Name ?? \"\"}]")]
    public class BcsComponentNode : BcsEntityNode
    {
        public BcsLocationNode Location => ParentNode as BcsLocationNode;

        public List<BcsComponentNode> SubComponents { get; private set; } = new List<BcsComponentNode>();
        public List<BcsAtomicAgentNode> AtomicAgents { get; private set; } = new List<BcsAtomicAgentNode>();

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return AtomicAgents.Concat(SubComponents.Cast<BcsExpressionNode>());
        }

    }
}