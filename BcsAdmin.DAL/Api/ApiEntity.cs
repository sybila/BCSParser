using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.DAL.Api
{
    public class ApiQueryEntity : IEntity<int>
    {
        public int Id {get; set;}
        public string Code { get; set; }
        public string Name { get; set; }
        public ApiEntityType? Type { get; set; }
        public ApiState State { get; set; }
    }


    public class ApiEntity : BcsObject
    {
        public string Code { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ApiEntityType? Type { get; set; }
        public IList<ApiEntityAnotation> Annotations { get; set; }

        public IList<int> Children { get; set; }
        public IList<int> Compartments { get; set; }

        //for agents
        public IList<int> Parents { get; set; }

        //for compartments 
        public int? Parent { get; set; }

        public IList<ApiState> States { get; set; }
    }
}
