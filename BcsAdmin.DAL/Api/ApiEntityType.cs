using System.Runtime.Serialization;

namespace BcsAdmin.DAL.Api
{
    public enum ApiEntityType
    {
        [EnumMember(Value = "state")]
        State,
        [EnumMember(Value = "compartment")]
        Compartment,
        [EnumMember(Value = "complex")]
        Complex,
        [EnumMember(Value = "structure")]
        Structure,
        [EnumMember(Value = "atomic")]
        Atomic
    }
}
