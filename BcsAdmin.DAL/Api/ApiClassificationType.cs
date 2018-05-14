using System.Runtime.Serialization;

namespace BcsAdmin.DAL.Api
{
    public enum ApiClassificationType
    {
        [EnumMember(Value = "entity")]
        Entity = 1,
        [EnumMember(Value = "reaction")]
        Reaction = 2,
        [EnumMember(Value = "rule")]
        Rule = 3
    }
}