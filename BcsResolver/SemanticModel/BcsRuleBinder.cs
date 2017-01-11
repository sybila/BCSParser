using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Extensions;
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

        public BcsBoundSymbol<BcsRuleSymbol> BindRule(BcsExpressionNode expression)
        {
            List<SemanticError> errors = new List<SemanticError>();
            var reaction = expression as BcsReactionNode;


            var ruleSymbol = new BcsRuleSymbol()
            {

            };

            foreach (var reactant in reaction.EnumerateChildNodes())
            {
                
            }


            return new BcsBoundSymbol<BcsRuleSymbol>()
            {
                Symbol = ruleSymbol,
                Errors = errors,
                Syntax = expression
            };
        }
    }
}
