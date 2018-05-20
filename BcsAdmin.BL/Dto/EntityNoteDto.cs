using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.BL.Dto
{
    public class EntityNoteDto : IEntity<int>
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        [Editable(false)]
        [DisplayFormat(NullDisplayText = "-")]
        public string UserName { get; set; }


        [Editable(false)]
        [DisplayFormat(NullDisplayText = "-")]
        public DateTime? Inserted { get; set; } = null;

        [Editable(false)]
        [DisplayFormat(NullDisplayText = "-")]
        public DateTime? Updated { get; set; }
    }
}
