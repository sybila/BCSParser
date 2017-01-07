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
        public void Parser_SimpleComposition_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS(Thr{p},Tyr{u})::cyt");

            var componentNode = tree.AssertCast<BcsAccessorNode>().Target
                .AssertCast<BcsComponentNode>();

            var pStateName = componentNode.Parts.AssertCount(2).ElementAt(0)
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1).ElementAt(0)
                .AssertCast<BcsAgentStateNode>().Name.AssertCast<BcsIdentifierNode>();
            Assert.AreEqual("p", pStateName.Name);

            var uStateName = componentNode.Parts.AssertCount(2).ElementAt(1)
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1).ElementAt(0)
                .AssertCast<BcsAgentStateNode>().Name.AssertCast<BcsIdentifierNode>();
            Assert.AreEqual("u", uStateName.Name);

            var cytName = tree.AssertCast<BcsAccessorNode>().Name.AssertCast<BcsIdentifierNode>();
            Assert.AreEqual("cyt", cytName.Name);
            Assert.AreEqual(new TextRange(3,1), componentNode.BeginBrace);
            Assert.AreEqual(new TextRange(17, 1), componentNode.EndBrace);
        }

        [TestMethod]
        public void Parser_SimpleAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("Tyr{p}");

            var agentNode = tree
                .AssertCast<BcsAtomicAgentNode>();

            var pStateName = agentNode.Parts.AssertCount(1).ElementAt(0)
               .AssertCast<BcsAgentStateNode>().Name.AssertCast<BcsIdentifierNode>();
            Assert.AreEqual("p", pStateName.Name);
            Assert.AreEqual(new TextRange(3,1), agentNode.BeginBrace);
            Assert.AreEqual(new TextRange(5, 1), agentNode.EndBrace);
        }

        [TestMethod]
        public void Parser_QualifiedName_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("Thr{u}::GS::cyt");

            var stateNameId = tree
                .AssertCast<BcsAccessorNode>().Target
                .AssertCast<BcsAccessorNode>().Target
                .AssertCast<BcsAtomicAgentNode>().Parts.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>().Name;

            Assert.AreEqual("u", stateNameId.Name);
        }

        [TestMethod]
        public void Parser_SimpleComplex_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS(Thr{p}).M(Tyr{u})::cyt");

            var complex = 
                tree.AssertCast<BcsAccessorNode>().Target
                .AssertCast<BcsComplexNode>();

            var frs = complex.Parts.AssertCount(2).ElementAt(0).AssertCast<BcsComponentNode>();
            var m = complex.Parts.ElementAt(1).AssertCast<BcsComponentNode>();

            var tyrP = frs.Parts.AssertCount(1).ElementAt(0).AssertCast<BcsAtomicAgentNode>();
            var tyrU = m.Parts.AssertCount(1).ElementAt(0).AssertCast<BcsAtomicAgentNode>();

            var p = tyrP.Parts.Single().AssertCast<BcsAgentStateNode>();
            var u = tyrU.Parts.Single().AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("FRS", frs.Name.Name);
            Assert.AreEqual("M", m.Name.Name);

            Assert.AreEqual("Tyr", tyrP.Name.Name);
            Assert.AreEqual("Tyr", tyrU.Name.Name);


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

            var r1Name = firstReactantLeft.Complex.AssertCast<BcsAccessorNode>().Target.AssertCast<BcsIdentifierNode>().Name;
            var r2Name = secondReactantLeft.Complex.AssertCast<BcsAccessorNode>().Target.AssertCast<BcsIdentifierNode>().Name;
            var l1Name = reactantRight.Complex.AssertCast<BcsAccessorNode>().Target.AssertCast<BcsIdentifierNode>().Name;

            Assert.AreEqual("FRS",r1Name);
            Assert.AreEqual("R", r2Name);
            Assert.AreEqual("FRSR", l1Name);
        }

        [TestMethod]
        public void Parser_SimpleReaction_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("FRS<=FRSR");
            var reactantLeft = tree.AssertCast<BcsReactionNode>().LeftSideReactants.AssertCount(1).ElementAt(0);
            var reactantRight = tree.AssertCast<BcsReactionNode>().RightSideReactants.AssertCount(1).ElementAt(0);

            Assert.AreEqual("FRS", reactantLeft.Complex.AssertCast<BcsIdentifierNode>().Name);
            Assert.AreEqual("FRSR", reactantRight.Complex.AssertCast<BcsIdentifierNode>().Name);
        }

        [TestMethod]
        public void Parser_SimpleComplexWhitespaced_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS   (   Thr  { p } )  .  Tyr { u  }   ::  M   ::  cyt");
           
        }
    }
}
