using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BcsResolver.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BcsResolver.File;
using BcsResolver.Syntax;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;
using BcsResolver.Tests.Helpers;
using Moq;

namespace BcsResolver.Tests
{
    [TestClass]
    public class BcsParserTest
    {
        [TestMethod]
        public void Parser_StructuralAgentTwoAtomicAgentsInAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS(Thr{p},Tyr{u})::cyt");

            var componentNode = tree.AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsStructuralAgentNode>();

            var pStateName = componentNode.Parts.AssertCount(2).ElementAt(0)
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1).ElementAt(0)
                .AssertCast<BcsAgentStateNode>().Identifier;
            Assert.AreEqual("p", pStateName.Name);

            var uStateName = componentNode.Parts.AssertCount(2).ElementAt(1)
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1).ElementAt(0)
                .AssertCast<BcsAgentStateNode>().Identifier;
            Assert.AreEqual("u", uStateName.Name);

            var cytName = tree.AssertCast<BcsContentAccessNode>().Container.AssertCast<BcsNamedEntityReferenceNode>().Identifier;
            Assert.AreEqual("cyt", cytName.Name);
            Assert.AreEqual(new TextRange(3,1), componentNode.BeginBrace);
            Assert.AreEqual(new TextRange(17, 1), componentNode.EndBrace);

            Assert.AreEqual("FRS(Thr{p},Tyr{u})::cyt", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_SimpleAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("Tyr{p}");

            var agentNode = tree
                .AssertCast<BcsAtomicAgentNode>();

            var pStateName = agentNode.Parts.AssertCount(1).ElementAt(0)
               .AssertCast<BcsAgentStateNode>().Identifier.AssertCast<BcsIdentifierNode>();
            Assert.AreEqual("p", pStateName.Name);
            Assert.AreEqual(new TextRange(3,1), agentNode.BeginBrace);
            Assert.AreEqual(new TextRange(5, 1), agentNode.EndBrace);
            Assert.AreEqual("Tyr{p}",tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_StructuralAgentCompositionInAtomicAgentAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("A{i}::K(B{u})::c");

            var i = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var u = tree
               .AssertCast<BcsContentAccessNode>().Target
               .AssertCast<BcsContentAccessNode>().Container
               .AssertCast<BcsStructuralAgentNode>().Parts.AssertCount(1)[0]
               .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
               .AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("i", i.Identifier.Name);
            Assert.AreEqual("u", u.Identifier.Name);
            Assert.AreEqual("A{i}::K(B{u})::c", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_AtomicAgentInAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("Thr{u}::GS::cyt");

            var stateNameId = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>().Identifier;

            Assert.AreEqual("u", stateNameId.Name);
            Assert.AreEqual("Thr{u}::GS::cyt", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_TwoStructuralAgentsInComplexAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS(Thr{p}).M(Tyr{u})::cyt");

            var complex = 
                tree.AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsComplexNode>();

            var frs = complex.Parts.AssertCount(2).ElementAt(0).AssertCast<BcsStructuralAgentNode>();
            var m = complex.Parts.ElementAt(1).AssertCast<BcsStructuralAgentNode>();

            var thrP = frs.Parts.AssertCount(1).ElementAt(0).AssertCast<BcsAtomicAgentNode>();
            var tyrU = m.Parts.AssertCount(1).ElementAt(0).AssertCast<BcsAtomicAgentNode>();

            var p = thrP.Parts.Single().AssertCast<BcsAgentStateNode>();
            var u = tyrU.Parts.Single().AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("FRS", frs.Identifier.Name);
            Assert.AreEqual("M", m.Identifier.Name);

            Assert.AreEqual("Thr", thrP.Identifier.Name);
            Assert.AreEqual("Tyr", tyrU.Identifier.Name);

            Assert.AreEqual("p", p.Identifier.Name);
            Assert.AreEqual("u", u.Identifier.Name);
            
            Assert.AreEqual("FRS(Thr{p}).M(Tyr{u})::cyt", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_ReactionBothSides_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("2.0 FRS::cyt+ 4R::cyt<=> 1.55FRSR::cyt");

            var firstReactantLeft = tree.AssertCast<BcsReactionNode>().LeftSideReactants.AssertCount(2).ElementAt(0);
            var secondReactantLeft = tree.AssertCast<BcsReactionNode>().LeftSideReactants.AssertCount(2).ElementAt(1);
            var reactantRight = tree.AssertCast<BcsReactionNode>().RightSideReactants.AssertCount(1).ElementAt(0);

            Assert.AreEqual(2.0, firstReactantLeft.Coeficient);
            Assert.AreEqual(4, secondReactantLeft.Coeficient);
            Assert.AreEqual(1.55, reactantRight.Coeficient);

            var r1Identifier = firstReactantLeft.Complex.AssertCast<BcsContentAccessNode>().Target.AssertCast<BcsNamedEntityReferenceNode>().Identifier;
            var r2Identifier = secondReactantLeft.Complex.AssertCast<BcsContentAccessNode>().Target.AssertCast<BcsNamedEntityReferenceNode>().Identifier;
            var l1Identifier = reactantRight.Complex.AssertCast<BcsContentAccessNode>().Target.AssertCast<BcsNamedEntityReferenceNode>().Identifier;

            Assert.AreEqual("FRS",r1Identifier.Name);
            Assert.AreEqual("R", r2Identifier.Name);
            Assert.AreEqual("FRSR", l1Identifier.Name);

            Assert.AreEqual("2FRS::cyt+4R::cyt<=>1.55FRSR::cyt", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_SimpleReactionLeft_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("FRS<=FRSR");
            var reactantLeft = tree.AssertCast<BcsReactionNode>().LeftSideReactants.AssertCount(1).ElementAt(0);
            var reactantRight = tree.AssertCast<BcsReactionNode>().RightSideReactants.AssertCount(1).ElementAt(0);

            Assert.AreEqual("FRS", reactantLeft.Complex.AssertCast<BcsNamedEntityReferenceNode>().Identifier.Name);
            Assert.AreEqual("FRSR", reactantRight.Complex.AssertCast<BcsNamedEntityReferenceNode>().Identifier.Name);

            Assert.AreEqual("1FRS<=1FRSR", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_ComplexFullySpecifiedInAtomicAgentComplexAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("B{p}::M::K.M.N::c");
            var p = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var kmn = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsComplexNode>().Parts.AssertCount(3);

            var k = kmn[0].AssertCast<BcsNamedEntityReferenceNode>();
            var m = kmn[1].AssertCast<BcsNamedEntityReferenceNode>();
            var n = kmn[2].AssertCast<BcsNamedEntityReferenceNode>();

            Assert.AreEqual("p", p.Identifier.Name);
            Assert.AreEqual("K", k.Identifier.Name);
            Assert.AreEqual("M", m.Identifier.Name);
            Assert.AreEqual("N", n.Identifier.Name);
            Assert.AreEqual("B{p}::M::K.M.N::c", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_AtomicAgentComplexAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("B{p}::M::X::c");
            var p = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("p", p.Identifier.Name);
            Assert.AreEqual("B{p}::M::X::c", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_EmptyBracketsInAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("A::B()::C.D.E::F");
            var a = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsNamedEntityReferenceNode>();

            Assert.AreEqual("A", a.Identifier.Name);
            Assert.AreEqual("A::B()::C.D.E::F", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_AtomicAgentStateSpecifiedInComplexInAtomicAgentAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("B{p}::M::K.M.N(C{a})::c");
            var p = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var a = tree
                .AssertCast<BcsContentAccessNode>().Target
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsComplexNode>().Parts.AssertCount(3)[2]
                .AssertCast<BcsStructuralAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("p", p.Identifier.Name);
            Assert.AreEqual("a", a.Identifier.Name);
            Assert.AreEqual("B{p}::M::K.M.N(C{a})::c", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_SimpleComplexWhitespaced_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS   (   Thr  { p } )  .  Tyr { u  }   ::  M   ::  cyt");
           

        }
    }
}
