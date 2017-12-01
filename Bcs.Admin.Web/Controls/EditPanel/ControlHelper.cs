using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;

namespace Bcs.Admin.Web.Controls.EditPanel
{
    public static class ControlHelper
    {
        public static HtmlGenericControl CreateDivWithClass(string classValue, params DotvvmControl[] children)
        {
            var div = new HtmlGenericControl("div");
            div.Attributes["class"] = classValue;

            foreach (var c in children)
            {
                div.Children.Add(c);
            }

            return div;
        }

    }
}
