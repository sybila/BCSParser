using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpNumbersGroupMembers
    {
        public int GroupId { get; set; }
        public int? MemberId { get; set; }

        public EpNumbersGroups Group { get; set; }
    }
}
