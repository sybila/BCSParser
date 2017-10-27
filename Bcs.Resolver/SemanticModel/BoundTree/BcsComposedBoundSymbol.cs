using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BcsResolver.SemanticModel.Tree;

namespace BcsResolver.SemanticModel.BoundTree
{
    [DebuggerDisplay("[BAA: {ToString()}]")]
    public class BcsBoundAtomicAgent : BcsComposedBoundSymbol<BcsAtomicAgentSymbol>
    {
    }

    [DebuggerDisplay("[BSA: {ToString()}]")]
    public class BcsBoundStructuralAgent : BcsComposedBoundSymbol<BcsStructuralAgentSymbol>
    { }

    [DebuggerDisplay("[BCX: {ToString()}]")]
    public class BcsBoundComplex : BcsComposedBoundSymbol<BcsComplexSymbol>
    { }


    [DebuggerDisplay("[BRE: {ToString()}]")]
    public class BcsBoundReaction : BcsComposedBoundSymbol<BcsRuleSymbol>
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

        public override IEnumerable<IBcsBoundSymbol> GetChildren() => StatedContent.Values.SelectMany(c=> c);
    }

    public interface IBcsComposedBoundSymbol : IBcsBoundSymbol
    {
        Dictionary<string, List<IBcsBoundSymbol>> StatedContent { get; }

        void AddContent(string name, IBcsBoundSymbol content);
    }
}
