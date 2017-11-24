using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace Bcs.Admin.Web.Controls
{
    public class GridViewEx : GridView
    {
        [MarkupOptions(MappingMode = MappingMode.InnerElement, Required = true)]
        public ITemplate FooterTemplate
        {
            get { return GetValue<ITemplate>(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }
        public static readonly DotvvmProperty FooterTemplateProperty
            = DotvvmProperty.Register<ITemplate, GridViewEx>(c => c.FooterTemplate, null);

        private DotvvmControl tfoot;

        protected override void OnInit(IDotvvmRequestContext context)
        {
            base.OnInit(context);

            if (FooterTemplate != null)
            {
                tfoot = BuildFooter(context);
            }
        }

        protected override void RenderContents(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            base.RenderContents(writer, context);

            if (tfoot != null)
            {
                tfoot.Render(writer, context);
            }
        }

        private HtmlGenericControl BuildFooter(IDotvvmRequestContext context)
        {
            var tfoot = new HtmlGenericControl("tfoot");
            var tr = new HtmlGenericControl("tr");
            tfoot.Children.Add(tr);
            var td = new HtmlGenericControl("td");
            td.DataContext = DataContext;
            td.Attributes["colspan"] = $"{Columns.Count}";
            tr.Children.Add(td);

            FooterTemplate.BuildContent(context, td);
            Children.Add(tfoot);

            return tfoot;
        }
    }
}
