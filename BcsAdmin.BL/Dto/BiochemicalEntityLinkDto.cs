using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.BL.Dto
{
    public abstract class BiochemicalEntityLinkDto : IAssociatedEntity
    {
        [Display(AutoGenerateField =false)]
        public int Id { get; set; }
        [Display(AutoGenerateField = false)]
        public int? IntermediateEntityId { get; set; }
        [Display(Name="Hierarchy type")]
        public int HierarchyType { get; set; }
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class LocationLinkDto : BiochemicalEntityLinkDto { }
    public class ComponentLinkDto : BiochemicalEntityLinkDto { }

    public class StateEntityDto : IAssociatedEntity
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }
        [Display(AutoGenerateField = false)]
        public int? IntermediateEntityId { get; set; } = null;
        [Display(Name = "Hierarchy type")]
        public int HierarchyType { get; } = 0; //satate
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
