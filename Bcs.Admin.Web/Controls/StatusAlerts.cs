using DotVVM.Framework.Binding;
using DotVVM.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls
{
    public class StatusAlerts : DotvvmMarkupControl
    {
        [MarkupOptions(Required = true)]
        public string SuccessMessage
        {
            get { return (string)GetValue(SuccessMessageProperty); }
            set { SetValue(SuccessMessageProperty, value); }
        }
        public static readonly DotvvmProperty SuccessMessageProperty
            = DotvvmProperty.Register<string, StatusAlerts>(c => c.SuccessMessage, null);

        [MarkupOptions(Required = true)]
        public List<string> Errors
        {
            get { return (List<string>)GetValue(ErrorsProperty); }
            set { SetValue(ErrorsProperty, value); }
        }
        public static readonly DotvvmProperty ErrorsProperty
            = DotvvmProperty.Register<List<string>, StatusAlerts>(c => c.Errors, new List<string>());
    }
}
