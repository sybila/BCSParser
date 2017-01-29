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
        public void SemanticAnalysisVisitor_BindSimpleStructuralAgent_ErrorNoLocation()
        {
            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.StructuralAgents).Returns(new Dictionary<string, BcsStructuralAgentSymbol>
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

            var tree = BcsSyntaxFactory.ParseModifier("A(B{u})");

            var bound = binder.Visit(tree);


            var uSymbol = bound
                .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].Single()
                .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].Single()
                .AssertCast<BcsBoundAgentState>().Symbol;

            Assert.AreEqual("u", uSymbol.Name);
            Assert.AreEqual("A(B{u})", bound.AssertCast<BcsBoundStructuralAgent>().Syntax.ToDisplayString());
            Assert.AreEqual(bound.AssertCast<BcsBoundStructuralAgent>().Syntax,
                binder.Errors.Keys.AssertCount(1).Single());
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_BindSimpleComplexExplicitComponents_ErrorNoLocation()
        {
            var complexTree = TestCaseFactory.CreateThreePartComplex();

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol>
            {
                {
                    "cx1",
                    complexTree
                }
            });

            var binder = new SemantiVisitor(mock.Object);

            var tree = BcsSyntaxFactory.ParseModifier("C().A(B{u})");

            var bound = binder.Visit(tree);

            var boundASymbol = bound
                .AssertCast<BcsBoundComplex>().StatedContent["A"].Single()
                .AssertCast<BcsBoundStructuralAgent>();

            var uSymbol = boundASymbol
                .StatedContent["B"].Single()
                .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].Single()
                .AssertCast<BcsBoundAgentState>().Symbol;

            var cSymbol = bound
                .AssertCast<BcsBoundComplex>().StatedContent["C"].Single().Symbol.AssertCast<BcsStructuralAgentSymbol>();

            Assert.AreEqual("C", cSymbol.Name);
            Assert.AreEqual("u", uSymbol.Name);
            Assert.AreEqual("A(B{u})", boundASymbol.Syntax.ToDisplayString());
            Assert.AreEqual(bound.AssertCast<BcsBoundComplex>().Syntax,
                binder.Errors.Keys.AssertCount(1).Single());
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_BindSimpleComplexWithLocation_Valid()
        {
            var locs = new List<BcsLocationSymbol> { new BcsLocationSymbol { Name = "cyt" } };
            var complexTree = TestCaseFactory.CreateThreePartComplex(locs);

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol>
            {
                {
                    "cx1",
                    complexTree
                }
            });
            mock.SetupGet(p => p.Locations).Returns(locs.ToDictionary(k => k.Name));

            var binder = new SemantiVisitor(mock.Object);

            var tree = BcsSyntaxFactory.ParseModifier("C.A(B{u})::cyt");

            var bound = binder.Visit(tree);

            var boundU = bound
                .AssertCast<BcsBoundLocation>().Content
                .AssertCast<BcsBoundComplex>().StatedContent["A"].AssertCount(1).Single()
                .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].AssertCount(1).Single()
                .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_BindSimpleExplicitComplexWithStateAccess_Valid()
        {
            var locs = new List<BcsLocationSymbol> { new BcsLocationSymbol { Name = "cyt" } };
            var complexTree = TestCaseFactory.CreateThreePartComplex(locs);

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol>
            {
                {
                    "cx1",
                    complexTree
                }
            });
            mock.SetupGet(p => p.Locations).Returns(locs.ToDictionary(k => k.Name));

            var binder = new SemantiVisitor(mock.Object);

            var tree = BcsSyntaxFactory.ParseModifier("B{u}::A::C.A::cyt");

            var bound = binder.Visit(tree);

            var boundU = bound
               .AssertCast<BcsBoundLocation>().Content
               .AssertCast<BcsBoundComplex>().StatedContent["A"].AssertCount(2).Last()
               .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].AssertCount(1).Single()
               .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_BindSimpleStateAccessComplexReference_Valid()
        {
            var locs = new List<BcsLocationSymbol> { new BcsLocationSymbol { Name = "cyt" } };
            var complexTree = TestCaseFactory.CreateThreePartComplex(locs);

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol> { { "cx1", complexTree } });
            mock.SetupGet(p => p.StructuralAgents).Returns(complexTree.StructuralAgents.ToDictionary(sa=> sa.Name));
            mock.SetupGet(p => p.AtomicAgents).Returns(complexTree.StructuralAgents.SelectMany(c=> c.AtomicAgents).ToDictionary(aa => aa.Name));
            mock.SetupGet(p => p.Locations).Returns(locs.ToDictionary(k => k.Name));

            var binder = new SemantiVisitor(mock.Object);

            var tree = BcsSyntaxFactory.ParseModifier("B{u}::A::cx1::cyt");

            var bound = binder.Visit(tree);

            var boundU = bound
               .AssertCast<BcsBoundLocation>().Content
               .AssertCast<BcsBoundComplex>().StatedContent["A"].AssertCount(1).Last()
               .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].AssertCount(1).Single()
               .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());
        }
    }
}
