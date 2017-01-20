using System.ComponentModel;

namespace BcsResolver.Syntax.Tokenizer
{
    public enum BcsExpresionTokenType
    {
        [Description("Id")]
        Identifier,
        [Description("WS")]
        Whitespace,
        [Description("AgB")]
        AgentBegin, //{
        [Description("AgE")]
        AgentEnd, //}
        [Description("AgStSep")]
        AgentSeparator, // |
        [Description("CoB")]
        ComponentBegin, //(
        [Description("CoE")]
        ComponentEnd, //)
        [Description(".")]
        Dot, //.
        [Description(",")]
        Comma, //,
        [Description("In")]
        Interaction, //+
        [Description("ReDL")]
        ReactionDirectionLeft, //<=
        [Description("ReDR")]
        ReactionDirectionRight, //=>
        [Description("ReDB")]
        ReactionDirectionBoth, //<=>
        [Description("ReCo")]
        ReactionCoeficient, //number
        [Description("VDeSep")]
        VariableDefinitionSeparator,//;
        [Description("::")]
        FourDot, //::
        [Description("?")]
        QuestionMark,
        [Description("=")]
        Assignment,
        [Description("Inv")]
        Invalid
            
    }
}
