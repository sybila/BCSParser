using System.ComponentModel;

namespace BcsResolver.Syntax.Tokenizer
{
    public enum BcsExpresionTokenType
    {
        [Description("Id")]
        Identifier,
        [Description("WS")]
        Whitespace,
        [Description("{")]
        SetBegin,
        [Description("}")]
        SetEnd,
        [Description("|")]
        AgentSeparator, 
        [Description("(")]
        BracketBegin, 
        [Description(")")]
        BracketEnd, 
        [Description(".")]
        Dot,
        [Description(",")]
        Comma,
        [Description("+")]
        Interaction,
        [Description("<=")]
        ReactionDirectionLeft,
        [Description("=>")]
        ReactionDirectionRight,
        [Description("<=>")]
        ReactionDirectionBoth,
        [Description("ReCo")]
        ReactionCoeficient,
        [Description("::")]
        FourDot,
        [Description("?")]
        QuestionMark,
        [Description(";")]
        Semicolon,
        [Description("=")]
        Assignment,
        [Description("Inv")]
        Invalid
    }
}
