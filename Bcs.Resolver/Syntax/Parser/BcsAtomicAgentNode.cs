﻿using System.Diagnostics;
using System.Linq;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[AA: {ToDisplayString()}]")]
    public sealed class BcsAtomicAgentNode : BcsComposedEntityNode
    {
        public override string ToDisplayString() => 
            $"{Identifier?.ToDisplayString()?? ""}{Parts.ToDisplayString()}";
    }
}