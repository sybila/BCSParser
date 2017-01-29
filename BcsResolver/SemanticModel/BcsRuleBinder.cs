using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Extensions;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax.Parser;

namespace BcsResolver.SemanticModel
{
    public class BcsRuleBinder
    {
        private readonly IBcsRuleMetadataProvider ruleMetadataProvider;

        public BcsRuleBinder(IBcsRuleMetadataProvider ruleMetadataProvider)
        {
            this.ruleMetadataProvider = ruleMetadataProvider;
        }
    }
}
