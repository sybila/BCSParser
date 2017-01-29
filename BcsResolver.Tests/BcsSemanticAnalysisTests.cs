using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.Tree;
using BcsResolver.Syntax;
using BcsResolver.Syntax.Visitors;
using BcsResolver.Tests.Helpers;
using Castle.Components.DictionaryAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BcsResolver.Tests
{
    [TestClass]
    public class BcsSemanticAnalysisTests
    {
        [TestMethod]
        public void Delf()
        {
            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.StructuralAgents ).Returns( new Dictionary<string, BcsStructuralAgentSymbol>
                {
                    {
                        "A",
                        new BcsStructuralAgentSymbol
                        {
                            Name = "A",
                            Locations = new List<BcsLocationSymbol> {new BcsLocationSymbol {Name = "cyt"}},
                            Parts = new List<BcsNamedSymbol>
                            {
                                new BcsAtomicAgentSymbol
                                {
                                    Name = "B",
                                    Parts = new List<BcsNamedSymbol>
                                    {
                                        new BcsStateSymbol {Name = "u"},
                                        new BcsStateSymbol {Name = "p"},
                                        new BcsStateSymbol {Name = "q"}
                                    }
                                }
                            }
                        }
                    }
                });

            var binder = new SemantiVisitor(mock.Object);

            var tree = BcsSyntaxFactory.ParseModifier("A(B{m})");

            var bound = binder.Visit(tree);
        }

    }
}
