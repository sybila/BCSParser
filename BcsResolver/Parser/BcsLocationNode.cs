using BcsResolver.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Parser
{
    public class BcsLocationNode : BcsEntityNode
    {
        //counts as child
        public BcsEntityNode Resident { get; set; }

        public override IEnumerable<BcsExpressionNode> EnumerateChildNodes()
        {
            return new BcsEntityNode[] { Resident };
        }
    }
}
