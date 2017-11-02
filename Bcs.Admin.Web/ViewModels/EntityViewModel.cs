using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BcsAnalysisWeb.ViewModels
{
    public class EntityViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public List<string> Children { get; set; }
    }
}