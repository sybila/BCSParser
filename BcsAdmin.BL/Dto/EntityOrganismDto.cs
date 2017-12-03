using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Riganti.Utils.Infrastructure.Core;

namespace BcsAdmin.BL.Dto
{
    public class EntityOrganismDto : IEntity<int>, IAssociatedEntity
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }
        [Display(AutoGenerateField = false)]
        public int? IntermediateEntityId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string GeneGroup { get; set; }
    }
}
