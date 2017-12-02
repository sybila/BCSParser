﻿using Riganti.Utils.Infrastructure.Core;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.BL.Dto
{
    public abstract class BiochemicalEntityLinkDto : IAssociatedEntity
    {
        [Display(AutoGenerateField =false)]
        public int Id { get; set; }
        [Display(AutoGenerateField = false)]
        public int? DetailEntityId { get; set; }
        [Display(Name="Hierarchy type")]
        public int HierarchyType { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class LocationLinkDto : BiochemicalEntityLinkDto { }
    public class ComponentLinkDto : BiochemicalEntityLinkDto { }
}
