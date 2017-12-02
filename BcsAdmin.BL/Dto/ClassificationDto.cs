using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.BL.Dto
{
    public class ClassificationDto : IAssociatedEntity
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }
        [Display(AutoGenerateField = false)]
        public int? DetailEntityId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
