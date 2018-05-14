using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using BcsAdmin.DAL.Api;

namespace BcsAdmin.BL.Dto
{
    public class ClassificationDto : IAssociatedEntity, IEntity<int>
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }
        [Display(AutoGenerateField = false)]
        public int? IntermediateEntityId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Type { get; set; }
    }
}
