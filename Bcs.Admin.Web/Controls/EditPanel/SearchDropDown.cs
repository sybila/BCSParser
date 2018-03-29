using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bcs.Admin.Web.ViewModels;
using BcsAdmin.BL.Dto;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Binding.Properties;
using DotVVM.Framework.Compilation;
using DotVVM.Framework.Compilation.ControlTree;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace Bcs.Admin.Web.Controls.EditPanel
{
    public class SearchDropDown : HtmlGenericControl
    {
        public enum Size {
            Small,
            Normal,
            Large
        }

        public Size TextBoxSize
        {
            get { return (Size)GetValue(TextBoxSizeProperty); }
            set { SetValue(TextBoxSizeProperty, value); }
        }
        public static readonly DotvvmProperty TextBoxSizeProperty
            = DotvvmProperty.Register<Size, SearchDropDown>(c => c.TextBoxSize, Size.Normal);


        public SearchDropDown() : base("div")
        {

        }


        /// <summary>
        ///   <div DataContext="{value: Components.EntitySearchSelect}" class="dropdown">
            //    <div class="has-feedback" Class-open="{value: Suggestions.Count != 0 && SelectedLink == null }" Class-has-success="{value: SelectedLink != null}">
            //        <cc:TextBoxEx class="form-control" Text="{value: Text}" KeyUp="{command:  RefreshSuggestions()}" data-toggle="dropdown" />
            //        <span Class-glyphicon-ok="{value: SelectedLink != null}" class="glyphicon form-control-feedback"></span>
            //        <dot:Repeater DataSource = "{value: Suggestions}" WrapperTagName="ul" class="dropdown-menu">
            //            <li><dot:LinkButton Text = "{value: Code+"("+Name+")"}" Click="{command: _parent.Select(_this)}" /></li>
            //        </dot:Repeater>
            //    </div>
            //</div>
        /// </summary>
        /// <param name="context"></param>
        protected override void OnInit(IDotvvmRequestContext context)
        {
            Attributes["class"] = "dropdown";



            if (!(DataContext is SearchSelect))
            {
                throw new InvalidOperationException($"DataContext mustbe of type {nameof(SearchSelect)}");
            }

            var selectedLinkNotNull = CreateValueBinding(context, $"{nameof(SearchSelect.SelectedSuggestion)} != null");
            var suggestionsBinding = CreateValueBinding(context, nameof(SearchSelect.Suggestions));

            var textBinding = CreateValueBinding(context, "Filter.SearchText");
            var keyUpBinding = CreateCommandBinding(context, "RefreshSuggestions()");

            var textbox = new TextBoxEx();
            textbox.Attributes["class"] = $"form-control { GetSizeClass()}";
            textbox.Attributes["data-toggle"] = "dropdown";
            textbox.SetBinding(TextBox.TextProperty,textBinding);
            textbox.SetBinding(TextBoxEx.KeyUpProperty, keyUpBinding);

            var iconSpan = new HtmlGenericControl("span");
            iconSpan.Attributes["class"] = "glyphicon form-control-feedback";
            iconSpan.SetBinding(CssClassesGroupDescriptor.GetDotvvmProperty("glyphicon-ok"), selectedLinkNotNull);

            var repeater = new Repeater();
            repeater.SetBinding(ItemsControl.DataSourceProperty, suggestionsBinding);
            repeater.WrapperTagName = "ul";
            repeater.Attributes["class"] = $"dropdown-menu";
            repeater.Attributes["style"] = $"top:30px";

            repeater.ItemTemplate = new DelegateTemplate((cbf, sp, c) =>
                {
                    var dcts = DataContextStack.Create(suggestionsBinding.ResultType.GenericTypeArguments[0], this.GetDataContextType());

                    c.SetDataContextType(dcts);

                    var selectBinding = ControlCreationHelper.CreateCommandBinding(context, dcts, "_parent.Select(_this)");
                    var nameBinding = ControlCreationHelper.CreateValueBinding(context, dcts, $"{nameof(SuggestionDto.Name)}+\"(\"+{nameof(SuggestionDto.Description)}+\")\"");

                    var linkButton = new LinkButton();
                    linkButton.SetBinding(ButtonBase.ClickProperty, selectBinding);
                    linkButton.SetBinding(ButtonBase.TextProperty, nameBinding);

                    var li = new HtmlGenericControl("li");
                    li.Children.Add(linkButton);
                    c.Children.Add(li);
                });

            var classOpenBinding = CreateValueBinding(context, $"{nameof(SearchSelect.Suggestions)}.Count != 0 && {nameof(SearchSelect.SelectedSuggestion)} == null");

            var buttonGroup = ControlCreationHelper.CreateDivWithClass("has-feedback");
            buttonGroup.SetBinding(CssClassesGroupDescriptor.GetDotvvmProperty("open"), classOpenBinding);
            buttonGroup.SetBinding(CssClassesGroupDescriptor.GetDotvvmProperty("has-success"), selectedLinkNotNull);
            buttonGroup.Children.Add(textbox);
            buttonGroup.Children.Add(iconSpan);
            buttonGroup.Children.Add(repeater);

            Children.Add(buttonGroup);

            base.OnInit(context);
        }

        private IValueBinding CreateValueBinding(IDotvvmRequestContext context, string bindingText)
        {
            return ControlCreationHelper.CreateValueBinding(context, this.GetDataContextType(), bindingText);
        }
        private ICommandBinding CreateCommandBinding(IDotvvmRequestContext context, string bindingText)
        {
            return ControlCreationHelper.CreateCommandBinding(context, this.GetDataContextType(), bindingText);
        }

        private string GetSizeClass()
        {
            switch (TextBoxSize)
            {
                case Size.Small:
                    return "input-sm";
                case Size.Large:
                    return "input-lg";
                default:
                    return "";
            }
        }
    }
}
