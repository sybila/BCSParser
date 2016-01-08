using BcsResolver.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.File
{
    public class BcsDefinitionFile
    {
        public List<BcsAgentStateNode> States { get; set; } = new List<BcsAgentStateNode>();
        public List<BcsAtomicAgentNode> AtomicAgents { get; private set; } = new List<BcsAtomicAgentNode>();
        public List<BcsComponentNode> Components { get; private set; } = new List<BcsComponentNode>();
        public List<BcsComplexNode> Complexes { get; private set; } = new List<BcsComplexNode>();
        public List<BcsLocationNode> Locations { get; private set; } = new List<BcsLocationNode>();

        public List<BcsEntity> Entities { get; private set; } = new List<BcsEntity>();
        public List<BcsRule> Rules { get; private set; } = new List<BcsRule>();
        public List<string> Artifacts { get; private set; } = new List<string>();
    }
}
