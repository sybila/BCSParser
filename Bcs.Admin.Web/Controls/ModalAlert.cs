using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls
{
    public class ModalAlert : DotvvmMarkupControl
    {
        public string ConfirmText
        {
            get { return (string)GetValue(ConfirmTextProperty); }
            set { SetValue(ConfirmTextProperty, value); }
        }
        public static readonly DotvvmProperty ConfirmTextProperty
            = DotvvmProperty.Register<string, ModalAlert>(c => c.ConfirmText, "");

        public Command ConfirmCommand
        {
            get { return (Command)GetValue(ConfirmCommandProperty); }
            set { SetValue(ConfirmCommandProperty, value); }
        }
        public static readonly DotvvmProperty ConfirmCommandProperty
            = DotvvmProperty.Register<Command, ModalAlert>(c => c.ConfirmCommand, null);


    }
}
