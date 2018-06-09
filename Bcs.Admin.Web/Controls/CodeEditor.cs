using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;

namespace Bcs.Admin.Web.Controls
{
    public class CodeEditor : HtmlGenericControl
    {
        public Command KeyDown
        {
            get { return (Command)GetValue(KeyDownProperty); }
            set { SetValue(KeyDownProperty, value); }
        }
        public static readonly DotvvmProperty KeyDownProperty
            = DotvvmProperty.Register<Command, CodeEditor>(c => c.KeyDown, null);

        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }
        public static readonly DotvvmProperty HtmlProperty
            = DotvvmProperty.Register<string, CodeEditor>(c => c.Html, null);


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DotvvmProperty TextProperty
            = DotvvmProperty.Register<string, CodeEditor>(c => c.Text, null);


        public CodeEditor()
            :base("div")
        {

        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.AddKnockoutDataBind("html", this, HtmlProperty, null, null, false, false);
            writer.AddKnockoutDataBind("htmlLazy", this, TextProperty, null, null, false, false);
            writer.AddKnockoutDataBind("contentEditable", "true");

            var keyDownBinding = GetCommandBinding(KeyDownProperty);
            if (keyDownBinding != null)

            {
                var expression = KnockoutHelper.GenerateClientPostBackExpression(
                    nameof(KeyDown),
                    keyDownBinding,
                    this,
                    new PostbackScriptOptions {
                        IsOnChange = true
                    });

                //writer.AddAttribute("onkeydown", $"{expression}; return true;");
            }

            writer.AddAttribute("spellcheck", "false");
            writer.AddAttribute("contenteditable", "true");

            base.AddAttributesToRender(writer, context);
        }
    }
}
