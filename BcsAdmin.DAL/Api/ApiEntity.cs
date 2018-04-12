using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.DAL.Api
{


    public class ApiEntity : BcsObject
    {
        public string Code { get; set; }
        public ApiEntityType Type { get; set; }
        public IList<ApiEntityAnotation> Annotations { get; set; }

        public IList<int> Children { get; set; }
        public IList<int> Compartments { get; set; }
        public IList<int> Parent { get; set; }
        public IList<ApiState> States { get; set; }
    }
}
