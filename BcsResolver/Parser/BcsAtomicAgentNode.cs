using BcsResolver.Tokenizer;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BcsResolver.Parser
{
    [DebuggerDisplay("[A: {Name}...]")]
    public class BcsAtomicAgentNode : BcsEntityNode
    {
        public TextRange StateBeginRange { get; set; }
        public TextRange StateEndRange { get; set; }

        public BcsAgentStateNode CurrentState { get; set; }

        public List<BcsAgentStateNode> AllStates { get; private set; } = new List<BcsAgentStateNode>();

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return new List<BcsAgentStateNode>() { CurrentState }.Concat(AllStates);
        }
    }
}