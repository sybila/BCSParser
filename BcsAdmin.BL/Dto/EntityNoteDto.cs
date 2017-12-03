using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BcsAdmin.BL.Dto
{
    public class EntityNoteDto : IEntity<int>, IAssociatedEntity
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

        [Editable(false)]
        public string UserName { get; set; }

        public string Text { get; set; }

        [Editable(false)]
        public DateTime? Inserted { get; set; }

        [Editable(false)]
        public DateTime? Updated { get; set; }

        [Display(AutoGenerateField = false)]
        public int? IntermediateEntityId { get; set; }
    }
}
