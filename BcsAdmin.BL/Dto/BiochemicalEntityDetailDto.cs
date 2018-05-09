using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public class BiochemicalEntityDetailDto : IEntity<int>
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }
        [Display(Name = "Type")]
        public int SelectedHierarchyType { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }   
        public int Status { get; set; }
    }
}
