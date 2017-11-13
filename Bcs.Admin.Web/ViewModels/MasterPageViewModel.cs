using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotVVM.Framework.ViewModel;
using Riganti.Utils.Infrastructure;

namespace Bcs.Admin.Web.ViewModels
{
    public class Masterpage : DotvvmViewModelBase
    {
        public string Title { get; set; }
        public List<string> Errors { get; set; }
    }
}

