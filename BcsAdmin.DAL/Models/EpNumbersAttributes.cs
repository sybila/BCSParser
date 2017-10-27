using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpNumbersAttributes
    {
        public EpNumbersAttributes()
        {
            EpNumbersValues = new HashSet<EpNumbersValues>();
        }

        public int Id { get; set; }
        public int BcsId { get; set; }
        public sbyte BcsType { get; set; }
        public string Name { get; set; }
        public string Keywords { get; set; }
        public int GroupId { get; set; }
        public int UnitId { get; set; }

        public EpNumbersGroups Group { get; set; }
        public ICollection<EpNumbersValues> EpNumbersValues { get; set; }
    }
}
