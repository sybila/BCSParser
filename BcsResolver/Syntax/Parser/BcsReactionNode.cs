using System.Collections.Generic;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
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
    }
}
