using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;

namespace BcsAdmin.DAL.Models
{
    public partial class EpClassification : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public override string ToString() => $"{Type}: {Name}";
    }
}
