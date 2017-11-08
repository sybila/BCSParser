using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.BL.Dto
{
    public class BiochemicalEntityLinkDto
    {
        public int Id { get; set; }
        [Display(Name="Hierarchy type")]
        public int HierarchyType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
