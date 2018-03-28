using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Binding.Properties;
using DotVVM.Framework.Compilation;
using DotVVM.Framework.Compilation.ControlTree;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls.EditPanel
{
    public static class ControlCreationHelper
    {
        public static Button IconButton(string iconName, string buttonText, ICommandBinding command, string additionalCss = null)
        {
            var editIcon = new HtmlGenericControl("i");
            editIcon.Attributes["class"] = $"glyphicon glyphicon-{iconName}";

            var iconButton = new Button();
            iconButton.ButtonTagName = ButtonTagName.button;
            iconButton.Attributes["class"] = "btn btn-sm btn-info " + (additionalCss ?? "");
            iconButton.SetBinding(ButtonBase.ClickProperty, command);
            iconButton.Children.Add(editIcon);
            iconButton.Children.Add(new Literal(buttonText));

            return iconButton;
        }

        public static LinkButton IconLinkButton(string iconName, string buttonText, ICommandBinding command, string additionalCss = null)
        {
            var editIcon = new HtmlGenericControl("i");
            editIcon.Attributes["class"] = $"glyphicon glyphicon-{iconName}";

            var iconButton = new LinkButton();
            iconButton.Attributes["class"] = (additionalCss ?? "");
            iconButton.SetBinding(ButtonBase.ClickProperty, command);
            iconButton.Children.Add(editIcon);
            iconButton.Children.Add(new Literal(buttonText));

            return iconButton;
        }

        public static HtmlGenericControl IconToggleLink(string iconName, string buttonText, string targetId, string toggleClass, string additionalCss = null)
        {
            var editIcon = new HtmlGenericControl("i");
            editIcon.Attributes["class"] = $"glyphicon glyphicon-{iconName}";

            var toggleButton = new HtmlGenericControl("a");
            toggleButton.Attributes["data-toggle"] = toggleClass;
            toggleButton.Attributes["data-target"] = $"#{targetId}";
            toggleButton.Attributes["role"] = "button";
            toggleButton.Attributes["class"] = (additionalCss ?? "");
            toggleButton.Children.Add(editIcon);
            toggleButton.Children.Add(new Literal(buttonText));

            return toggleButton;
        }



        public static IValueBinding CreateValueBinding(IDotvvmRequestContext context, DataContextStack contextTypeStack, string bindingText)
        {
            var bindingService = (BindingCompilationService)context.Services.GetService(typeof(BindingCompilationService));
            return new ValueBindingExpression(
                bindingService,
                new object[] {
                    new BindingParserOptions(typeof(ValueBindingExpression)),
                    new OriginalStringBindingProperty(bindingText),
                    contextTypeStack
                });
        }

        public static ICommandBinding CreateCommandBinding(IDotvvmRequestContext context, DataContextStack contextTypeStack, string bindingText)
        {
            var bindingService = (BindingCompilationService)context.Services.GetService(typeof(BindingCompilationService));
            var bindingId = Convert.ToBase64String(Encoding.ASCII.GetBytes(contextTypeStack.DataContextType.Name + "." + bindingText));
            var properties = new object[]{
                contextTypeStack,
                new OriginalStringBindingProperty(bindingText),
                new IdBindingProperty(bindingId)
            };

            return new CommandBindingExpression(bindingService, properties);
        }

        public static ICommandBinding CreateStaticCommandBinding(IDotvvmRequestContext context, DataContextStack contextTypeStack, string bindingText)
        {
            var bindingService = (BindingCompilationService)context.Services.GetService(typeof(BindingCompilationService));
            var bindingId = Convert.ToBase64String(Encoding.ASCII.GetBytes(contextTypeStack.DataContextType.Name + "." + bindingText));
            var properties = new object[]{
                contextTypeStack,
                new OriginalStringBindingProperty(bindingText),
                new IdBindingProperty(bindingId)
            };

            return new StaticCommandBindingExpression(bindingService, properties);
        }


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
