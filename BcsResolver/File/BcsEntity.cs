using BcsResolver.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public class BcsEntity : BcsFileRecord
    {
        public BcsExpressionAdapter<BcsExpressionNode> States { get; private set; } = new BcsExpressionAdapter<BcsExpressionNode>();
        public List<string> Locations { get; private set; } = new List<string>();
        public BcsExpressionAdapter<BcsExpressionNode> Composition { get; private set; } = new BcsExpressionAdapter<BcsExpressionNode>();
    }
}
