using BcsResolver.Tokenizer;
using System.Collections.Generic;
using System.Diagnostics;

namespace BcsResolver.Parser
{
    [DebuggerDisplay("Components = [{string.Join(\", \",Components.Select(c=> c?.Name ?? \"()\"))}]")]
    public class BcsComplexNode : BcsEntityNode
    {
        public List<BcsEntityNode> Components { get; private set; } = new List<BcsEntityNode>();
        public List<TextRange> Separators { get; private set; } = new List<TextRange>();

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return Components;
        }
    }
}