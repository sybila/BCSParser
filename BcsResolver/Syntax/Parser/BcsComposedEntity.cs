﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    public abstract class BcsComposedEntity<TComponent> : BcsNamedEntityNode
        where TComponent : BcsNamedEntityNode
    {
        public BcsSet<TComponent> Parts { get; set; }
        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return base.EnumerateChildNodes().Concat(new []{ Parts });
        }
    }
}