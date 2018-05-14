using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.DAL.Api
{
    public class ApiClassification : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ApiClassificationType Type { get; set; }
    }
}
