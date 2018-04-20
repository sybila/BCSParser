using System.Runtime.Serialization;

namespace BcsAdmin.DAL.Api
{
    public enum ApiEntityStatus
    {
        [EnumMember(Value = "1")]
        Pending,
        [EnumMember(Value = "2")]
        Active,
        [EnumMember(Value = "3")]
        Inactive
    }
}
