using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Api
{
    public class BcsObject : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<int> Classifications { get; set; }
        public IList<int> Organisms { get; set; }
        public IList<ApiEntityAnotation> Annotations { get; set; }


        [JsonConverter(typeof(StringEnumConverter))]
        public ApiEntityStatus? Status { get; set; }
    }
}
