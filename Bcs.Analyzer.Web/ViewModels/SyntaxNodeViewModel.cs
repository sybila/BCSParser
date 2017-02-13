using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BcsAnalysisWeb.ViewModels
{
    public class SyntaxNodeViewModel
    {
        public string NodeName { get; set; }
        public string Dispaly { get; set; }
        public bool HasErrors => Errors.Any();
        public List<string> Errors { get; set; }
    }
}