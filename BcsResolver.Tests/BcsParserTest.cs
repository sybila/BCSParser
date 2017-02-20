using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BcsResolver.Common;
using BcsResolver.Extensions;
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

            var componentNode = tree.AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsStructuralAgentNode>();

            var pStateName = componentNode.Parts.Elements.AssertCount(2).ElementAt(0)
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1).ElementAt(0)
                .AssertCast<BcsAgentStateNode>().Identifier;
            Assert.AreEqual("p", pStateName.Name);

            var uStateName = componentNode.Parts.Elements.AssertCount(2).ElementAt(1)
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1).ElementAt(0)
                .AssertCast<BcsAgentStateNode>().Identifier;
            Assert.AreEqual("u", uStateName.Name);

            var cytName = tree.AssertCast<BcsContentAccessNode>().Container.AssertCast<BcsNamedEntityReferenceNode>().Identifier;
            Assert.AreEqual("cyt", cytName.Name);
            Assert.AreEqual(new TextRange(3,1), componentNode.Parts.OpeningToken.ToTextRange());
            Assert.AreEqual(new TextRange(17, 1), componentNode.Parts.ClosingToken.ToTextRange());

            Assert.AreEqual("FRS(Thr{p},Tyr{u})::cyt", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_SimpleAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("Tyr{p}");

            var agentNode = tree
                .AssertCast<BcsAtomicAgentNode>();

            var pStateName = agentNode.Parts.Elements.AssertCount(1).ElementAt(0)
               .AssertCast<BcsAgentStateNode>().Identifier.AssertCast<BcsIdentifierNode>();
            Assert.AreEqual("p", pStateName.Name);
            Assert.AreEqual(new TextRange(3,1), agentNode.Parts.OpeningToken.ToTextRange());
            Assert.AreEqual(new TextRange(5, 1), agentNode.Parts.ClosingToken.ToTextRange());
            Assert.AreEqual("Tyr{p}",tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_StructuralAgentCompositionInAtomicAgentAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("A{i}::K(B{u})::c");

            var i = tree
                .AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var u = tree
               .AssertCast<BcsContentAccessNode>().Container
               .AssertCast<BcsContentAccessNode>().Child
               .AssertCast<BcsStructuralAgentNode>().Parts.Elements.AssertCount(1)[0]
               .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
               .AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("i", i.Identifier.Name);
            Assert.AreEqual("u", u.Identifier.Name);
            Assert.AreEqual("A{i}::K(B{u})::c", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_AtomicAgentInAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("Thr{u}::GS::cyt");

            var u = tree
                .AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("u", u.Identifier.Name);
            Assert.AreEqual("Thr{u}::GS::cyt", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_TwoStructuralAgentsInComplexAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS(Thr{p}).M(Tyr{u})::cyt");

            var complex = 
                tree.AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsComplexNode>();

            var frs = complex.Parts.Elements.AssertCount(2).ElementAt(0).AssertCast<BcsStructuralAgentNode>();
            var m = complex.Parts.Elements.ElementAt(1).AssertCast<BcsStructuralAgentNode>();

            var thrP = frs.Parts.Elements.AssertCount(1).ElementAt(0).AssertCast<BcsAtomicAgentNode>();
            var tyrU = m.Parts.Elements.AssertCount(1).ElementAt(0).AssertCast<BcsAtomicAgentNode>();

            var p = thrP.Parts.Elements.Single().AssertCast<BcsAgentStateNode>();
            var u = tyrU.Parts.Elements.Single().AssertCast<BcsAgentStateNode>();

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

            var r1Identifier = firstReactantLeft.Complex.AssertCast<BcsContentAccessNode>().Child.AssertCast<BcsNamedEntityReferenceNode>().Identifier;
            var r2Identifier = secondReactantLeft.Complex.AssertCast<BcsContentAccessNode>().Child.AssertCast<BcsNamedEntityReferenceNode>().Identifier;
            var l1Identifier = reactantRight.Complex.AssertCast<BcsContentAccessNode>().Child.AssertCast<BcsNamedEntityReferenceNode>().Identifier;

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
                .AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var kmn = tree
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsComplexNode>().Parts.Elements.AssertCount(3);

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
                .AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var c = tree
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsNamedEntityReferenceNode>();

            Assert.AreEqual("p", p.Identifier.Name);
            Assert.AreEqual("c", c.Identifier.Name);
            Assert.AreEqual("B{p}::M::X::c", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_EmptyBracketsInAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("A::B()::C.D.E::F");
            var f = tree
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsNamedEntityReferenceNode>();

            Assert.AreEqual("F", f.Identifier.Name);
            Assert.AreEqual("A::B()::C.D.E::F", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_AtomicAgentStateSpecifiedInComplexInAtomicAgentAccessor_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("B{p}::M::K.M.N(C{a})::c");
            var p = tree
                .AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var a = tree
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Container
                .AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsComplexNode>().Parts.Elements.AssertCount(3)[2]
                .AssertCast<BcsStructuralAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("p", p.Identifier.Name);
            Assert.AreEqual("a", a.Identifier.Name);
            Assert.AreEqual("B{p}::M::K.M.N(C{a})::c", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_SimmplestReactionWithVariable_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A<=>?Var;Var={B,C,D}");

            var variableExpression = tree.AssertCast<BcsVariableExpresssioNode>();
            variableExpression.References.SeparatorTokens.AssertCount(2);

            var a = variableExpression.TargetExpression
                .AssertCast<BcsReactionNode>().LeftSideReactants.AssertCount(1)[0]
                .AssertCast<BcsReactantNode>().Complex
                .AssertCast<BcsNamedEntityReferenceNode>();

            var b =variableExpression.References.Elements.AssertCount(3)[0].AssertCast<BcsNamedEntityReferenceNode>();
            var varDef = variableExpression.VariableName;

            Assert.AreEqual("A", a.Identifier.Name);
            Assert.AreEqual("B", b.Identifier.Name);
            Assert.AreEqual("Var", varDef.Name);
            Assert.AreEqual("1A<=>1?Var;Var={B,C,D}", tree.ToDisplayString());

            Assert.AreEqual(new TextRange(8,1),variableExpression.DefinitionSeparator);
            Assert.AreEqual(new TextRange(12, 1), variableExpression.AssignmentOperator);
            Assert.AreEqual(new TextRange(15, 1), variableExpression.References.SeparatorTokens[0].ToTextRange());
        }

        [TestMethod]
        public void Parser_WithVariableInComplex_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A::c<=>?Var.SA()::c;Var={D,E,F}");

            Assert.AreEqual("1A::c<=>1?Var.SA()::c;Var={D,E,F}", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_WithVariableInStructuralAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A::c<=>B.SA(?Var)::c;Var={D,E,F}");

            Assert.AreEqual("1A::c<=>1B.SA(?Var)::c;Var={D,E,F}", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_WithVariableInAtomicAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A::c<=>AA{?Var}::SA::Complex::c;Var={D,E,F}");

            Assert.AreEqual("1A::c<=>1AA{?Var}::SA::Complex::c;Var={D,E,F}", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_WithVariableAsAtomicAgentInsideStructuralAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A::c<=>B.SA(AA{?Var})::c;Var={D,E,F}");

            Assert.AreEqual("1A::c<=>1B.SA(AA{?Var})::c;Var={D,E,F}", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_TwoVariablesInComplex_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A::c<=>?Var.B.?Var.SA()::c;Var={D}");

            Assert.AreEqual("1A::c<=>1?Var.B.?Var.SA()::c;Var={D}", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_TwoVariablesInStructuralAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A::c<=>B.SA(?Var,AA{p},?Var)::c;Var={D}");

            Assert.AreEqual("1A::c<=>1B.SA(?Var,AA{p},?Var)::c;Var={D}", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_TwoVariablesInAtomicAgent_Valid()
        {
            var tree = BcsSyntaxFactory.ParseReaction("A::c<=>AA{?Var,p,?Var}::SA::B.SA::c;Var={D}");

            Assert.AreEqual("1A::c<=>1AA{?Var,p,?Var}::SA::B.SA::c;Var={D}", tree.ToDisplayString());
        }

        [TestMethod]
        public void Parser_SimpleComplexComposedOfStructuralAndAtimicAgents_Valid()
        {
            var tree = BcsSyntaxFactory.ParseModifier("FRS(Thr{p}).Tyr{u}::M::cyt");

            var complexParts =tree.AssertCast<BcsContentAccessNode>().Child
                .AssertCast<BcsComplexNode>().Parts.Elements.AssertCount(2);

            var pState = complexParts[0].AssertCast<BcsStructuralAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            var uState = complexParts[1].AssertCast<BcsAtomicAgentNode>().Parts.Elements.AssertCount(1)[0]
                .AssertCast<BcsAgentStateNode>();

            Assert.AreEqual("p", pState.Identifier.Name);
            Assert.AreEqual("u", uState.Identifier.Name);

            Assert.AreEqual("FRS(Thr{p}).Tyr{u}::M::cyt", tree.ToDisplayString());
        }
    }
}
