using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    public abstract class BcsComposedEntityNode : BcsNamedEntityNode
    {
        public BcsNamedEntitySet Parts { get; set; }
        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes().Concat(new []{ Parts });
        }
    }
}