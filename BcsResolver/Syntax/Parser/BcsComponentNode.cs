﻿using System.Diagnostics;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Syntax.Parser
{
    [DebuggerDisplay("[{Name}::{Location?.Name ?? \"\"}]")]
    public sealed class BcsComponentNode : BcsComposedEntity
    {
        public TextRange BeginBrace { get; set; }
        public TextRange EndBrace { get; set; }
    }
}