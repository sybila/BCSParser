using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BcsResolver.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BcsResolver.File;
using BcsResolver.SemanticModel;
using BcsResolver.Syntax.Tokenizer;
using BcsResolver.Tests.Helpers;
using Moq;

namespace BcsResolver.Tests
{
    [TestClass]
    public class BcsTokenizerTest
    {
        [TestMethod]
        public void Tokenizer_SimpleComposition_Valid()
        {
            var tokens = CreateTokens("FRS(Thr{p},Tyr{u})::cyt");
            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.BracketBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetEnd,
                BcsExpresionTokenType.Comma,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetEnd,
                BcsExpresionTokenType.BracketEnd,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_SimpleComplex_Valid()
        {
            var tokens = CreateTokens("FRS(Thr{p}).M(Tyr{u})::cyt");
            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.BracketBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetEnd,
                BcsExpresionTokenType.BracketEnd,
                BcsExpresionTokenType.Dot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.BracketBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetEnd,
                BcsExpresionTokenType.BracketEnd,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_QualifiedName_Valid()
        {
            var tokens = CreateTokens("Thr{u}::GS::cyt");
            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetEnd,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));

        }

        [TestMethod]
        public void Tokenizer_ReactionBothSides_Valid()
        {
            var tokens = CreateTokens("FRS::cyt + R::cyt<=>FRSR::cyt");
            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.ReactionDirectionBoth,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_ReactionWithCoeficients_Valid()
        {
            var tokens = CreateTokens("2.0 FRS::cyt + 4R::cyt<=>1.55FRSR::cyt");
            new[]
            {
                BcsExpresionTokenType.ReactionCoeficient, 
                BcsExpresionTokenType.Whitespace, 
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.ReactionCoeficient,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.ReactionDirectionBoth,
                BcsExpresionTokenType.ReactionCoeficient,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_ReactionWithCoeficientsAndComplexes_Valid()
        {
            var tokens = CreateTokens("2.0 FRS.GC::cyt + 4R.HJ::cyt<=>1.55FRSR.M::cyt");
            new[]
            {
                BcsExpresionTokenType.ReactionCoeficient,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Dot, 
                BcsExpresionTokenType.Identifier, 
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.ReactionCoeficient,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Dot, 
                BcsExpresionTokenType.Identifier, 
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.ReactionDirectionBoth,
                BcsExpresionTokenType.ReactionCoeficient,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Dot, 
                BcsExpresionTokenType.Identifier, 
                BcsExpresionTokenType.FourDot,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_ReactionLeft_Valid()
        {
            var tokens = CreateTokens("FRS<=FRSR");
            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.ReactionDirectionLeft,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_ReactionRight_Valid()
        {
            var tokens = CreateTokens("FRS=>FRSR");

            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.ReactionDirectionRight,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_PlusAsIdentifier_Valid()
        {
            var tokens = CreateTokens("FRS+ + H{ + }=>FHRS+ + H{+}");
            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetBegin,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.SetEnd,
                BcsExpresionTokenType.ReactionDirectionRight,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetBegin,
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.SetEnd,
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        [TestMethod]
        public void Tokenizer_SimpleComplexWhitespaced_Valid()
        {
            var tokens = CreateTokens("FRS   (   Thr  { p } )  .  Tyr { u  }   ::  M   ::  cyt");
            new[]
            {
                BcsExpresionTokenType.Identifier,
                BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.BracketBegin,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.SetBegin,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.SetEnd,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.BracketEnd,
                 BcsExpresionTokenType.Whitespace,

                BcsExpresionTokenType.Dot,
              
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.SetBegin,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.SetEnd,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.FourDot,
                 BcsExpresionTokenType.Whitespace,
                  BcsExpresionTokenType.Identifier,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.FourDot,
                 BcsExpresionTokenType.Whitespace,
                BcsExpresionTokenType.Identifier
            }
            .AssertSequenceEquals(tokens.Select(t => t.Type));
        }

        private List<BcsExpresionToken> CreateTokens(string text)
        {
            var tokenizer = new BcsExpresionTokenizer();
            tokenizer.Tokenize(new StringReader(text));
            return tokenizer.Tokens;
        }
    }
}
