using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.BL.Dto
{
    public abstract class BiochemicalEntityLinkDto : IAssociatedEntity, IEntity<int>
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

    public class StateEntityDto : IEntity<string>
    {
        [Required]
        [Display(Name = "Code")]
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
