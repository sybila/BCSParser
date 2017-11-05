using BcsAdmin.BL.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalEntityDetail
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Xml for visualisation")]
        public string VisualisationXml { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Required]
        [Display(Name = "Entity type")]
        public int SelectedHierarchyType { get; set; }

        [Display(AutoGenerateField = false)]
        public List<BiochemicalEntityTypeDto> HierarchyTypes { get; set; }

        [Display(AutoGenerateField = false)]
        public BiochemicalEntityLinkDto Parent { get; set; }

        [Display(AutoGenerateField = false)]
        public virtual ICollection<BiochemicalEntityLinkDto> Components { get; set; }

        [Display(AutoGenerateField = false)]
        public virtual ICollection<BiochemicalEntityLinkDto> Locations { get; set; }

        //public virtual ICollection<EpEntityClassification> Classifications { get; set; } = new List<EpEntityClassification>();
    }
}
