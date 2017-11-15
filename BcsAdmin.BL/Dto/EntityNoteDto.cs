using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace BcsAdmin.BL.Dto
{
    public class EntityNoteDto : IEntity<int>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime? Inserted { get; set; }
        public DateTime? Updated { get; set; }
    }
}
