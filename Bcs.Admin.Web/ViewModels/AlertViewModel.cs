using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.ViewModels
{
    public class AlertViewModel
    {
        public string AlertHeading { get; set; }
        public string AlertText { get; set; }
        public List<string> AlertItems { get; set; }
    }
}
