using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BcsResolver.Syntax.Parser;
using BcsResolver.Syntax.Tokenizer;

namespace BcsResolver.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDisplayString(this ReactionDirectionType type)
        {
            switch (type)
            {
                case ReactionDirectionType.Both:
                    return "<=>";
                case ReactionDirectionType.Left:
                    return "<=";
                case ReactionDirectionType.Right:
                    return "=>";
                default:
                    throw new InvalidOperationException($"Unsupported {nameof(ReactionDirectionType)}.");
            }
        }

        public static string ToDisplayString(this BcsExpresionTokenType tt)
        {
            return tt.GetDescription();
        }

        public static string ToDisplayString(this BcsExpresionToken tt)
        {
            return tt == null ? "" : tt.Type.GetDescription();
        }


        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }
    }
}
