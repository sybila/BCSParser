using Riganti.Utils.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bcs.Admin.Web.ViewModels
{
    public class BiochemicalEntityRowDto : IEntity<int>
    {
        [Display(AutoGenerateField = false)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Type { get; set; }

        public string EntityTypeCss { get; set; }

        public List<string> Children { get; set; }

        public List<string> Locations { get; set; }
    }
}
