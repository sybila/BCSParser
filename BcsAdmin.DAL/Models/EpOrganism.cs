using System;
using System.Collections.Generic;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.DAL.Models
{
    public partial class EpOrganism : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
