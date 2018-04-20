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
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class LocationLinkDto : BiochemicalEntityLinkDto
    {
    }
    public class ComponentLinkDto : BiochemicalEntityLinkDto
    {
        [Required]
        [Display(Name = "Agent Type")]
        public int? HierarchyType { get; set; } = null;

        public ComponentLinkDto()
        {

        }
    }

    public class StateEntityDto : IEntity<int>, IAssociatedEntity
    {
        public int Id { get; set; } = -1;
        public string Code { get; set; }
        public string Description { get; set; }
        public int? IntermediateEntityId { get; set; }
    }
}
