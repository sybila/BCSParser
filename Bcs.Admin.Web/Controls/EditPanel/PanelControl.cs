using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Binding.Expressions;
using Bcs.Admin.Web.Controls.EditPanel;

namespace Bcs.Admin.Web.Controls
{
    public class PanelControl : HtmlGenericControl
    {
        public string HeadingText
        {
            get { return (string)GetValue(HeadingTextProperty); }
            set { SetValue(HeadingTextProperty, value); }
        }
        public static readonly DotvvmProperty HeadingTextProperty =
            DotvvmProperty.Register<string, PanelControl>(t => t.HeadingText, "");

        [MarkupOptions(MappingMode = MappingMode.InnerElement)]
        public ITemplate BodyTemplate
        {
            get { return (ITemplate)GetValue(BodyTemplateProperty); }
            set { SetValue(BodyTemplateProperty, value); }
        }
        public static readonly DotvvmProperty BodyTemplateProperty =
            DotvvmProperty.Register<ITemplate, PanelControl>(t => t.BodyTemplate, null);

        [MarkupOptions(MappingMode = MappingMode.InnerElement)]
        public ITemplate FooterTemplate
        {
            get { return (ITemplate)GetValue(FooterTemplateProperty); }
            set { SetValue(FooterTemplateProperty, value); }
        }
        public static readonly DotvvmProperty FooterTemplateProperty =
            DotvvmProperty.Register<ITemplate, PanelControl>(t => t.FooterTemplate, null);

        [MarkupOptions(MappingMode = MappingMode.Attribute)]
        public ICommandBinding CloseCommand
        {
            get { return GetValue<ICommandBinding>(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DotvvmProperty CloseCommandProperty =
            DotvvmProperty.Register<ICommandBinding, PanelControl>(t => t.CloseCommand, null);

        public PanelControl()
            :base("div")
        {

        }

        protected override void OnInit(IDotvvmRequestContext context)
        {
            Attributes["class"] = " panel panel-info";

            var headingDiv = new HtmlGenericControl("div");
            headingDiv.Attributes["class"] = "panel-heading";
            headingDiv.Children.Add(new Literal(HeadingText));

            if(CloseCommand != null)
            {
                var closeButton = ControlCreationHelper.IconLinkButton("remove", " Close", CloseCommand, "pull-right");
                headingDiv.Children.Add(closeButton);
            }

            var bodyDiv = new HtmlGenericControl("div");
            bodyDiv.Attributes["class"] = "panel-body";

            var footerDiv = new HtmlGenericControl("div");
            footerDiv.Attributes["class"] = "panel-footer";

            Children.Add(headingDiv);
            Children.Add(bodyDiv);
            Children.Add(footerDiv);

            CreateContent(context, bodyDiv, footerDiv);

            base.OnInit(context);
        }

        protected virtual void CreateContent(IDotvvmRequestContext context, HtmlGenericControl bodyDiv, HtmlGenericControl footerDiv)
        {
            BodyTemplate?.BuildContent(context, bodyDiv);
            FooterTemplate?.BuildContent(context, footerDiv);
        }
    }
}
