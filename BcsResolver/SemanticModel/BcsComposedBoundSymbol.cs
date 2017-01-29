using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.SemanticModel.Tree;

namespace BcsResolver.SemanticModel
{
    public class BcsBoundAtomicAgent : BcsComposedBoundSymbol<BcsAtomicAgentSymbol>
    { }

    public class BcsBoundStructuralAgent : BcsComposedBoundSymbol<BcsStructuralAgentSymbol>
    { }
 
    public class BcsBoundComplex : BcsComposedBoundSymbol<BcsComplexSymbol>
    { }

    public class BcsBoundLocation : BcsComposedBoundSymbol<BcsLocationSymbol>
    { }

    public abstract class BcsComposedBoundSymbol<TSymbol> : BcsBoundSymbol<TSymbol>, IBcsComposedBoundSymbol
        where TSymbol : BcsSymbol
    {
        public Dictionary<string, List<IBcsBoundSymbol>> StatedContent { get; set; } = new Dictionary<string, List<IBcsBoundSymbol>>();

        public void AddContent(string name, IBcsBoundSymbol content)
        {
            if (!StatedContent.ContainsKey(name))
            {
                StatedContent[name] = new List<IBcsBoundSymbol>();
            }
            StatedContent[name].Add(content);
        }
    }

    public interface IBcsComposedBoundSymbol : IBcsBoundSymbol
    {
        Dictionary<string, List<IBcsBoundSymbol>> StatedContent { get; }

        void AddContent(string name, IBcsBoundSymbol content);
    }
}
