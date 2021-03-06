﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.SemanticModel.BoundTree;
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
                        Locations = new List<BcsNamedSymbol> {new BcsCompartmentSymbol {Name = "cyt"}},
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

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

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

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

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
            var locs = new List<BcsCompartmentSymbol> { new BcsCompartmentSymbol { Name = "cyt" } };
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

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

            var tree = BcsSyntaxFactory.ParseModifier("C.A(B{u})::cyt");

            var bound = binder.Visit(tree);

            var boundU = bound
                .AssertCast<BcsBoundLocation>().Content
                .AssertCast<BcsBoundComplex>().StatedContent["A"].AssertCount(1).Single()
                .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].AssertCount(1).Single()
                .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());

            Assert.AreEqual(0, binder.Errors.Count);
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_BindSimpleExplicitComplexWithStateAccess_Valid()
        {
            var locs = new List<BcsCompartmentSymbol> { new BcsCompartmentSymbol { Name = "cyt" } };
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

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

            var tree = BcsSyntaxFactory.ParseModifier("B{u}::A::C.A::cyt");

            var bound = binder.Visit(tree);

            var boundU = bound
               .AssertCast<BcsBoundLocation>().Content
               .AssertCast<BcsBoundComplex>().StatedContent["A"].AssertCount(2).Last()
               .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].AssertCount(1).Single()
               .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());

            Assert.AreEqual(0, binder.Errors.Count);
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_BindSimpleStateAccessComplexReference_Valid()
        {
            var locs = new List<BcsCompartmentSymbol> { new BcsCompartmentSymbol { Name = "cyt" } };
            var complexTree = TestCaseFactory.CreateThreePartComplex(locs);

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol> { { "cx1", complexTree } });
            mock.SetupGet(p => p.StructuralAgents).Returns(complexTree.StructuralAgents.ToDictionary(sa => sa.Name));
            mock.SetupGet(p => p.AtomicAgents).Returns(complexTree.StructuralAgents.SelectMany(c => c.AtomicAgents).ToDictionary(aa => aa.Name));
            mock.SetupGet(p => p.Locations).Returns(locs.ToDictionary(k => k.Name));

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

            var tree = BcsSyntaxFactory.ParseModifier("B{u}::A::cx1::cyt");

            var bound = binder.Visit(tree);

            var boundU = bound
               .AssertCast<BcsBoundLocation>().Content
               .AssertCast<BcsBoundComplex>().StatedContent["A"].AssertCount(1).Last()
               .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].AssertCount(1).Single()
               .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());

            Assert.AreEqual(0, binder.Errors.Count);
        }

        //Decide if I even want to deal with merging
        [Ignore]
        [TestMethod]
        public void SemanticAnalysisVisitor_BindComplexStateAccessComplexReference_Valid()
        {
            var locs = new List<BcsCompartmentSymbol> { new BcsCompartmentSymbol { Name = "cyt" } };
            var complexTree = TestCaseFactory.CreateThreePartComplex(locs);

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol> { { "cx1", complexTree } });
            mock.SetupGet(p => p.StructuralAgents).Returns(complexTree.StructuralAgents.ToDictionary(sa => sa.Name));
            mock.SetupGet(p => p.AtomicAgents).Returns(complexTree.StructuralAgents.SelectMany(c => c.AtomicAgents).ToDictionary(aa => aa.Name));
            mock.SetupGet(p => p.Locations).Returns(locs.ToDictionary(k => k.Name));

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

            var tree = BcsSyntaxFactory.ParseModifier("B{u}::A(G{p}, B{u})::A(G{q}).C(D{u}).E(F{u})::cyt");

            var bound = binder.Visit(tree);

            var boundU = bound
               .AssertCast<BcsBoundLocation>().Content
               .AssertCast<BcsBoundComplex>().StatedContent["A"].AssertCount(1).Last()
               .AssertCast<BcsBoundStructuralAgent>().StatedContent["B"].AssertCount(1).Single()
               .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());

            Assert.AreEqual(0, binder.Errors.Count);
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_BindComplexComposedOfStructuralAndAtomicAgent_Valid()
        {
            var complexTree = TestCaseFactory.CreateMixedAgentComplex();

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol> { { "mixed", complexTree } });
            mock.SetupGet(p => p.StructuralAgents).Returns(complexTree.Parts.OfType<BcsStructuralAgentSymbol>().ToDictionary(sa => sa.Name));
            mock.SetupGet(p => p.AtomicAgents).Returns(complexTree.Parts.OfType<BcsAtomicAgentSymbol>().Concat(complexTree.Parts.OfType<BcsStructuralAgentSymbol>().SelectMany(sa => sa.AtomicAgents)).ToDictionary(aa => aa.Name));
            mock.SetupGet(p => p.Locations).Returns(new [] { (BcsCompartmentSymbol)complexTree.Locations[0] }.ToDictionary(k => k.Name));

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

            var tree = BcsSyntaxFactory.ParseModifier("FRS(Thr{p}).Tyr{u}::cyt");

            var boundTree = binder.Visit(tree);

            var boundP = boundTree.AssertCast<BcsBoundLocation>().Content
               .AssertCast<BcsBoundComplex>().StatedContent["FRS"].AssertCount(1).Last()
               .AssertCast<BcsBoundStructuralAgent>().StatedContent["Thr"].AssertCount(1).Single()
               .AssertCast<BcsBoundAtomicAgent>().StatedContent["p"].AssertCount(1).Single();

            var boundU = boundTree.AssertCast<BcsBoundLocation>().Content
             .AssertCast<BcsBoundComplex>().StatedContent["Tyr"].AssertCount(1).Last()
             .AssertCast<BcsBoundAtomicAgent>().StatedContent["u"].AssertCount(1).Single();

            Assert.AreEqual("p", boundP.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("p", boundP.Syntax.ToDisplayString());

            Assert.AreEqual("u", boundU.Symbol.AssertCast<BcsNamedSymbol>().Name);
            Assert.AreEqual("u", boundU.Syntax.ToDisplayString());

            Assert.AreEqual(0, binder.Errors.Count);
        }

        [TestMethod]
        public void SemanticAnalysisVisitor_UnknownStructuralAgentKnownLoacation_Invalid()
        {
            var complexTree = TestCaseFactory.CreateMixedAgentComplex();

            var mock = new Mock<IBcsWorkspace>();
            mock.SetupGet(p => p.Complexes).Returns(new Dictionary<string, BcsComplexSymbol> { { "mixed", complexTree } });
            mock.SetupGet(p => p.StructuralAgents)
                .Returns(complexTree.Parts.OfType<BcsStructuralAgentSymbol>().ToDictionary(sa => sa.Name));
            mock.SetupGet(p => p.AtomicAgents)
                .Returns(
                    complexTree.Parts.OfType<BcsAtomicAgentSymbol>()
                        .Concat(complexTree.Parts.OfType<BcsStructuralAgentSymbol>().SelectMany(sa => sa.AtomicAgents))
                        .ToDictionary(aa => aa.Name));
            mock.SetupGet(p => p.LocationEntityMap)
                .Returns(new Dictionary<string, IReadOnlyList<BcsComposedSymbol>>());

            mock.SetupGet(p => p.Locations).Returns(new[] { (BcsCompartmentSymbol)complexTree.Locations[0] }.ToDictionary(k => k.Name));

            var binder = new SemanticAnalysisVisitor(mock.Object, new BcsBoundSymbolFactory());

            var tree = BcsSyntaxFactory.ParseModifier("bbb::aaa(aa)::cyt");

            var boundTree = binder.Visit(tree);


        }
    }
}
