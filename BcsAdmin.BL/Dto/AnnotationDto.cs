using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsAdmin.BL.Dto
{
    public class AnnotationDto : IEntity<int>
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
    }
}
