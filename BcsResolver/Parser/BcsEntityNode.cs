using BcsResolver.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Parser
{
    public abstract class BcsEntityNode : BcsExpressionNode
    {
        public string Name { get; set; }
        public TextRange NameRange { get; set; }
    }
}
