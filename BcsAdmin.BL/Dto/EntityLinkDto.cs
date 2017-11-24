using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public class EntityLinkDto
    {
        public int DetailId { get; set; }
        public int AssociatedId { get; set; }
    }
}
