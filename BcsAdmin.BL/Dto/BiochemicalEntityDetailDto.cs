using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public class BiochemicalEntityDetailDto : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int SelectedHierarchyType { get; set; }
    }
}
