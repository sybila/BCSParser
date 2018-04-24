using System.Runtime.Serialization;

namespace BcsAdmin.DAL.Api
{
    public enum ApiEntityStatus
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "active")]
        Active,
        [EnumMember(Value = "inactive")]
        Inactive
    }
}
