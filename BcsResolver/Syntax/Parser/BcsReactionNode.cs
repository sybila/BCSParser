using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.Extensions;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[R: {ToDisplayString()}]")]
    public class BcsReactionNode : BcsExpressionNode
    {
        public List<BcsReactantNode> LeftSideReactants { get; set; } = new List<BcsReactantNode>();
        public List<BcsReactantNode> RightSideReactants { get; set; } = new List<BcsReactantNode>();
        public ReactionDirectionType ReactionDirection { get; set; }

        public TextRange ReactionDirectionRange { get; set; }
        public List<TextRange> InteractionSeparatorRanges { get; private set; } = new List<TextRange>();

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return LeftSideReactants.Concat(RightSideReactants);
        }

        public override string ToDisplayString()
        {
            string left = string.Join("+", LeftSideReactants.Select(r => r.ToDisplayString()));
            string right = string.Join("+", RightSideReactants.Select(r => r.ToDisplayString()));

            return $"{left}{ReactionDirection.ToDisplayString()}{right}";
        }
    }
}
