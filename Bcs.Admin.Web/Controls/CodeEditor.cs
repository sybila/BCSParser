using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;

namespace Bcs.Analyzer.DemoWeb.Controls
{
    public class CodeEditor : HtmlGenericControl
    {
        public Command KeyUp
        {
            get { return (Command)GetValue(KeyUpProperty); }
            set { SetValue(KeyUpProperty, value); }
        }
        public static readonly DotvvmProperty KeyUpProperty
            = DotvvmProperty.Register<Command, CodeEditor>(c => c.KeyUp, null);

        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }
        public static readonly DotvvmProperty HtmlProperty
            = DotvvmProperty.Register<string, CodeEditor>(c => c.Html, null);


        public CodeEditor()
            :base("div")
        {

        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.AddKnockoutDataBind("htmlLazy", this, HtmlProperty, null, null, false, false);
            writer.AddKnockoutDataBind("contentEditable", "true");

            var keyUpBinding = GetCommandBinding(KeyUpProperty);
            if (keyUpBinding != null)
            {
                writer.AddAttribute("onkeyup", KnockoutHelper.GenerateClientPostBackScript(nameof(KeyUp), keyUpBinding, this, useWindowSetTimeout: true, isOnChange: true));
            }

            writer.AddAttribute("spellcheck", "false");
            writer.AddAttribute("contenteditable", "true");

            base.AddAttributesToRender(writer, context);
        }
    }
}
