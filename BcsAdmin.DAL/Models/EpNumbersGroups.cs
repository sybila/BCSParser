using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpNumbersGroups
    {
        public EpNumbersGroups()
        {
            EpNumbersAttributes = new HashSet<EpNumbersAttributes>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? SupervisorId { get; set; }

        public EpNumbersGroupMembers EpNumbersGroupMembers { get; set; }
        public ICollection<EpNumbersAttributes> EpNumbersAttributes { get; set; }
    }
}
