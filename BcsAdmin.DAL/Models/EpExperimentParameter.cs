using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpExperimentParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DataType { get; set; }
        public int? Order { get; set; }
        public int? Required { get; set; }
    }
}
