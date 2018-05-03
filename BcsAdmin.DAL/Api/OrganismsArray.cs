using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.DAL.Api
{
    public class OrganismArray : IEntity<int>
    {
        public int Id { get; set; }
        public List<int> Organisms { get; set; }
    }

    public class ClassificationArray : IEntity<int>
    {
        public int Id { get; set; }
        public List<int> Classifications { get; set; }
    }
}
