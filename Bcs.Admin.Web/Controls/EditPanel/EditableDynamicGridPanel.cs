using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;

namespace Bcs.Admin.Web.Controls.EditPanel
{
    public class EditableDynamicGridPanel : DynamicGridPanelBase
    {
        [MarkupOptions(AllowHardCodedValue = false)]
        [ControlPropertyBindingDataContextChange(nameof(DataSourceBinding))]
        [CollectionElementDataContextChange(1)]
        public ICommandBinding EditCommand
        {
            get { return GetValue<ICommandBinding>(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly DotvvmProperty EditCommandProperty =
            DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(t => t.EditCommand, null);

        [MarkupOptions(AllowHardCodedValue = false)]
        [ControlPropertyBindingDataContextChange(nameof(DataSourceBinding))]
        [CollectionElementDataContextChange(1)]
        public ICommandBinding DeleteCommand
        {
            get { return GetValue<ICommandBinding>(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DotvvmProperty DeleteCommandProperty =
            DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(t => t.DeleteCommand, null);

        [MarkupOptions(AllowHardCodedValue = false)]
        [ControlPropertyBindingDataContextChange(nameof(DataSourceBinding))]
        [CollectionElementDataContextChange(1)]
        public ICommandBinding CancelCommand
        {
            get { return GetValue<ICommandBinding>(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

        public static readonly DotvvmProperty CancelCommandProperty =
            DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(t => t.CancelCommand, null);


        [MarkupOptions(AllowHardCodedValue = false)]
        [ControlPropertyBindingDataContextChange(nameof(DataSourceBinding))]
        [CollectionElementDataContextChange(1)]
        public ICommandBinding SaveCommand
        {
            get { return (ICommandBinding)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }
        public static readonly DotvvmProperty SaveCommandProperty
            = DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(c => c.SaveCommand, null);

        protected override void SetUpToolColumn(GridViewTemplateColumn toolColumn)
        {
            toolColumn.ContentTemplate = new DelegateTemplate((cbf, sp,  c) => 
            {
                var editButton = CreateIconButton("edit", "", EditCommand);
                var removeButton = CreateIconButton("trash", "", DeleteCommand);

                c.Children.Add(editButton);
                c.Children.Add(removeButton);
            });

            toolColumn.EditTemplate = new DelegateTemplate((cbf, sp, c) => 
            {
                var cancelButton = CreateIconButton("remove", "", CancelCommand);
                var saveButton = CreateIconButton("save", "", SaveCommand);

                c.Children.Add(cancelButton);
                c.Children.Add(saveButton);
            });
        }

        protected override void SetUpFooter(IDotvvmRequestContext context, HtmlGenericControl footerDiv)
        {
            FooterTemplate.BuildContent(context,footerDiv);
        }

        protected virtual Button CreateIconButton(string iconName, string buttonText, ICommandBinding command)
        {
            var editIcon = new HtmlGenericControl("i");
            editIcon.Attributes["class"] = $"glyphicon glyphicon-{iconName}";

            var iconButton = new Button();
            iconButton.ButtonTagName = ButtonTagName.button;
            iconButton.Attributes["class"] = "btn btn-sm btn-info";
            iconButton.SetBinding(ButtonBase.ClickProperty, command);
            iconButton.Children.Add(editIcon);
            iconButton.Children.Add(new Literal(buttonText));

            return iconButton;
        }
    }
}
