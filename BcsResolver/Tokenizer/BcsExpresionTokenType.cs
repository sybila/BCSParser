using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsResolver.Tokenizer
{
    public enum BcsExpresionTokenType
    {
        [Description("AgId")]
        AgentIdentifier,
        [Description("WS")]
        Whitespace,
        [Description("AgB")]
        AgentStateBegin, //{
        [Description("AgStId")]
        AgentStateIdentifier,
        [Description("AgStE")]
        AgentStateEnd, //}
        [Description("AgStSep")]
        AgentSeparator, // |
        [Description("CsB")]
        CompositionBegin, //(
        [Description("CsE")]
        CompositionEnd, //)
        [Description("CsSep")]
        CompositionSeparator, //.
        [Description("AgInP")]
        AgentInteractionPositive, //+
        [Description("ReDL")]
        ReactionDirectionLeft, //<=
        [Description("ReDR")]
        ReactionDirectionRight, //=>
        [Description("ReDB")]
        ReactionDirectionBoth, //<=>
        [Description("ReCo")]
        ReactionCoeficient, //number
        [Description("VAgSep")]
        VariableAgentSeparator, //,
        [Description("VDeSep")]
        VariableDefinitionSeparator,//;
        [Description("IM")]
        InheritanceMark, //::

        //virtual - for parser
        [Description("CxB")]
        ComplexStart,
        [Description("CxE")]
        ComplexEnd,
        [Description("CtB")]
        ComponentStart,
        [Description("CtE")]
        ComponentEnd
    }
}
