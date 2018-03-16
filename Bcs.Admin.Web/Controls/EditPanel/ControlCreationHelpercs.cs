using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
