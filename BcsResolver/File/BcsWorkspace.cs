using BcsResolver.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.SemanticModel;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.File
{
    public class BcsWorkspace
    {
        private readonly IBcsEntityMetadataProvider entityMetadataProvider;
        private readonly IBcsRuleMetadataProvider ruleMetadataProvider;
        private BcsEntityBinder entityBinder;

        public BcsWorkspace(IBcsRuleMetadataProvider ruleMetadataProvider, IBcsEntityMetadataProvider entityMetadataProvider)
        {
            this.ruleMetadataProvider = ruleMetadataProvider;
            this.entityMetadataProvider = entityMetadataProvider;
        }

        public IReadOnlyDictionary<string, BcsComplexSymbol> Complexes { get; private set; }
        public IReadOnlyDictionary<string, BcsComponentSymbol> Commponents { get; private set; }
        public IReadOnlyDictionary<string, BcsAgentSymbol> Agents { get; private set; }

        public void CreateSemanticModel()
        {
            entityBinder = new BcsEntityBinder(entityMetadataProvider);

            var resolvedSymbols =entityBinder.BindEntities().ToLookup(k => k.Type);

            Complexes = resolvedSymbols[BcsSymbolType.Complex].OfType<BcsComplexSymbol>().ToDictionary(k=> k.Name);
            Commponents = resolvedSymbols[BcsSymbolType.Component].OfType<BcsComponentSymbol>().ToDictionary(k => k.Name);
            Agents = resolvedSymbols[BcsSymbolType.Agent].OfType<BcsAgentSymbol>().ToDictionary(k => k.Name);


        }


    }
}
