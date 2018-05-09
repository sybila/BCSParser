using System.Runtime.Serialization;

namespace BcsAdmin.DAL.Api
{
    public enum ApiEntityStatus
    {
        [EnumMember(Value = "pending")]
        Pending = 0,
        [EnumMember(Value = "active")]
        Active = 1,
        [EnumMember(Value = "inactive")]
        Inactive = 2
    }
}
