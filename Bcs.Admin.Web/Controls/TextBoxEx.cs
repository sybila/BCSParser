using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace Bcs.Admin.Web.Controls
{
    public class TextBoxEx : TextBox
    {
        [MarkupOptions(AllowHardCodedValue = false)]
        public ICommandBinding KeyUp
        {
            get { return GetValue<ICommandBinding>(KeyUpProperty); }
            set { SetValue(KeyUpProperty, value); }
        }
        public static readonly DotvvmProperty KeyUpProperty
            = DotvvmProperty.Register<ICommandBinding, TextBoxEx>(c => c.KeyUp, null);

        protected override void OnInit(IDotvvmRequestContext context)
        {
            UpdateTextAfterKeydown = true;
            base.OnInit(context);
        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            var keyUpBinding = GetCommandBinding(KeyUpProperty);
            if (keyUpBinding != null)
            {
                writer.AddAttribute("onkeyup", KnockoutHelper.GenerateClientPostBackScript(nameof(KeyUp), keyUpBinding, this, useWindowSetTimeout: true, isOnChange: true));
            }
         
            base.AddAttributesToRender(writer, context);
        }
    }
}
