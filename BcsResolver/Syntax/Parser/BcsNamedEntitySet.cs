using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.Extensions;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[Set count={Elements.Count}: {ToDisplayString()}]")]
    public class BcsNamedEntitySet : BcsExpressionNode
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public List<BcsNamedEntityNode> Elements { get; set; } = new List<BcsNamedEntityNode>();
        public List<BcsExpresionToken> SeparatorTokens { get; set; } = new List<BcsExpresionToken>();
        public BcsExpresionToken OpeningToken { get; set; }
        public BcsExpresionToken ClosingToken { get; set; }

        public override string ToDisplayString()
        {
            var parts = Elements
                .Select(e => e?.ToDisplayString() ?? "")
                .ToList()
                .Interleave(
                    SeparatorTokens
                    .Select(t => t?.ToDisplayString() ?? "")
                    .ToList());

            return $"{EnumExtensions.ToDisplayString(OpeningToken)}{string.Join("", parts)}{EnumExtensions.ToDisplayString(ClosingToken)}";
        }

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes().Concat(Elements);
        }
    }
}