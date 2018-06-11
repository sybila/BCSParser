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

        public IValueBinding Html
        {
            get { return (IValueBinding)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }
        public static readonly DotvvmProperty HtmlProperty
            = DotvvmProperty.Register<IValueBinding, CodeEditor>(c => c.Html, null);

        [MarkupOptions(AllowAttributeWithoutValue = false, AllowHardCodedValue = false)]
        public IValueBinding Text
        {
            get { return (IValueBinding)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DotvvmProperty TextProperty
            = DotvvmProperty.Register<IValueBinding, CodeEditor>(c => c.Text, null);

        [MarkupOptions(AllowAttributeWithoutValue = false, AllowHardCodedValue = false)]
        public IValueBinding StyleSpans
        {
            get { return (IValueBinding)GetValue(StyleSpansProperty); }
            set { SetValue(StyleSpansProperty, value); }
        }
        public static readonly DotvvmProperty StyleSpansProperty
            = DotvvmProperty.Register<IValueBinding, CodeEditor>(c => c.StyleSpans, null);

        public CodeEditor()
            :base("div")
        {

        }

        protected override void RenderBeginTag(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.WriteKnockoutDataBindComment(
                "dotvvm_introduceAlias",
                $"{{ '$codeEditor': new CodeEditor(), '$editorText': {Text.GetKnockoutBindingExpression(this)}, '$editorSpans': {StyleSpans.GetKnockoutBindingExpression(this)}}}");
            base.RenderBeginTag(writer, context);
        }

        protected override void RenderEndTag(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.RenderEndTag();
            writer.WriteKnockoutDataBindEndComment();
        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.AddKnockoutDataBind("html", this, HtmlProperty, null, null, false, false);
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

                var getEditor = "ko.contextFor(this).$codeEditor()";

                var handleBeforeKey = $"{getEditor}.handleBeforeKey(this)";
                var handleAfterKey = $"{getEditor}.handleAfterKey(this, {expression})";


                writer.AddAttribute("onkeyup", $"{handleBeforeKey}; {handleAfterKey}; return true;");
            }

            writer.AddAttribute("spellcheck", "false");
            writer.AddAttribute("contenteditable", "true");

            base.AddAttributesToRender(writer, context);
        }
    }
}
