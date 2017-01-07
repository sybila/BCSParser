using System.Collections.Generic;
using BcsResolver.File;

namespace BcsResolver.SemanticModel
{
    public interface IBcsRuleMetadataProvider
    {
        BcsRule GetRule(string id);
        IEnumerable<string> GetRulesToProcess();
    }
}