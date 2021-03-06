﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bcs.Admin.Web.ViewModels.Grids;
using BcsAdmin.BL.Dto;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Binding.Properties;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Hosting;

namespace Bcs.Admin.Web.Controls.EditPanel
{
    public class EditableDynamicGridPanel : DynamicGridPanelBase
    {

        [MarkupOptions(AllowHardCodedValue = false)]
        public IValueBinding LinkEntitySearchSelect
        {
            get { return GetValue<IValueBinding>(LinkEntitySearchSelectProperty); }
            set { SetValue(LinkEntitySearchSelectProperty, value); }
        }
        public static readonly DotvvmProperty LinkEntitySearchSelectProperty
            = DotvvmProperty.Register<IValueBinding, EditableDynamicGridPanel>(c => c.LinkEntitySearchSelect, null);

        [MarkupOptions(AllowHardCodedValue = false)]
        public ICommandBinding LinkEntity
        {
            get { return (ICommandBinding)GetValue(LinkEntityProperty); }
            set { SetValue(LinkEntityProperty, value); }
        }
        public static readonly DotvvmProperty LinkEntityProperty
            = DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(c => c.LinkEntity, null);



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

        [MarkupOptions(AllowHardCodedValue = false)]
        public ICommandBinding AddCommand
        {
            get { return (ICommandBinding)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }
        public static readonly DotvvmProperty AddCommandProperty
            = DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(c => c.AddCommand, null);

        [MarkupOptions(AllowHardCodedValue = false)]
        public ICommandBinding SaveNewCommand
        {
            get { return (ICommandBinding)GetValue(SaveNewCommandProperty); }
            set { SetValue(SaveNewCommandProperty, value); }
        }
        public static readonly DotvvmProperty SaveNewCommandProperty
            = DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(c => c.SaveNewCommand, null);

        [MarkupOptions(AllowHardCodedValue = false)]
        public ICommandBinding CancelNew
        {
            get { return (ICommandBinding)GetValue(CancelNewProperty); }
            set { SetValue(CancelNewProperty, value); }
        }
        public static readonly DotvvmProperty CancelNewProperty
            = DotvvmProperty.Register<ICommandBinding, EditableDynamicGridPanel>(c => c.CancelNew, null);


        [MarkupOptions(AllowHardCodedValue = false, Required = true)]
        public IValueBinding NewEntityFormVisible
        {
            get { return (IValueBinding)GetValue(NewEntityFormVisibleProperty); }
            set { SetValue(NewEntityFormVisibleProperty, value); }
        }
        public static readonly DotvvmProperty NewEntityFormVisibleProperty
            = DotvvmProperty.Register<IValueBinding, EditableDynamicGridPanel>(c => c.NewEntityFormVisible, null);

        [MarkupOptions(AllowHardCodedValue = false, Required = true)]
        public IValueBinding NewEntityDto
        {
            get { return (IValueBinding)GetValue(NewEntityDtoProperty); }
            set { SetValue(NewEntityDtoProperty, value); }
        }

        public BindingCompilationService BindingCompilationService { get; }

        public static readonly DotvvmProperty NewEntityDtoProperty
            = DotvvmProperty.Register<IValueBinding, EditableDynamicGridPanel>(c => c.NewEntityDto, null);

        [MarkupOptions(AllowHardCodedValue = false, Required = true)]
        public IValueBinding Errors
        {
            get { return (IValueBinding)GetValue(ErrorsProperty); }
            set { SetValue(ErrorsProperty, value); }
        }
        public static readonly DotvvmProperty ErrorsProperty
            = DotvvmProperty.Register<IValueBinding, EditableDynamicGridPanel>(c => c.Errors, null);

        [MarkupOptions(AllowHardCodedValue = false, Required = true)]
        public IValueBinding SuccessMessage
        {
            get { return (IValueBinding)GetValue(SuccessMessageProperty); }
            set { SetValue(SuccessMessageProperty, value); }
        }
        public static readonly DotvvmProperty SuccessMessageProperty
            = DotvvmProperty.Register<IValueBinding, EditableDynamicGridPanel>(c => c.SuccessMessage, null);


        public EditableDynamicGridPanel(BindingCompilationService bindingCompilationService)
        {
            BindingCompilationService = bindingCompilationService;
        }

        protected override void CreateContent(IDotvvmRequestContext context, HtmlGenericControl bodyDiv, HtmlGenericControl footerDiv)
        {
            var statusAlerts = new StatusAlerts();
            statusAlerts.SetBinding(StatusAlerts.ErrorsBindingProperty, Errors);
            statusAlerts.SetBinding(StatusAlerts.SuccessMessageBindingProperty, SuccessMessage);
            bodyDiv.Children.Add(statusAlerts);
            base.CreateContent(context, bodyDiv, footerDiv);
        }

        protected override void SetUpToolColumn(GridViewTemplateColumn toolColumn)
        {
            toolColumn.ContentTemplate = new DelegateTemplate((cbf, sp, c) =>
            {
                var editButton = CreateIconButton("edit", "", EditCommand);
                var removeButton = CreateIconButton("trash", "", DeleteCommand);

                c.Children.Add(editButton);
                c.Children.Add(new Literal(" "));
                c.Children.Add(removeButton);
            });

            toolColumn.EditTemplate = new DelegateTemplate((cbf, sp, c) =>
            {
                var cancelButton = CreateIconButton("remove", "", CancelCommand);
                var saveButton = CreateIconButton("save", "", SaveCommand);

                c.Children.Add(cancelButton);
                c.Children.Add(new Literal(" "));
                c.Children.Add(saveButton);
            });
        }

        protected override void SetUpFooter(IDotvvmRequestContext context, HtmlGenericControl footerDiv)
        {
            var leftDiv = CreateDivWithClass("col-md-9");

            if (LinkEntitySearchSelect != null && LinkEntity != null)
            {
                var inputGroupBtn = CreateDivWithClass("input-group-btn", CreateIconButton("link", "Link existing", LinkEntity));

                var searchDropDown = new SearchDropDown(BindingCompilationService);
                searchDropDown.TextBoxSize = SearchDropDown.Size.Small;
                searchDropDown.SetBinding(DataContextProperty, LinkEntitySearchSelect);
                searchDropDown.SetDataContextType(this.CreateChildStack(LinkEntitySearchSelect.ResultType));

                var inputGroup = CreateDivWithClass("input-group", searchDropDown, inputGroupBtn);

                leftDiv.Children.Add(inputGroup);
            }


            var rightDiv = CreateDivWithClass("col-md-3", CreateIconButton("plus", "Add new", AddCommand));
            var row = CreateDivWithClass("row", leftDiv, rightDiv);

            row.SetBinding(VisibleProperty, NewEntityFormVisible.GetProperty<NegatedBindingExpression>().Binding);

            var newEntityDtoStack = this.CreateChildStack(NewEntityDto.ResultType);

            var dynamicEntity = new DynamicEntity();
            dynamicEntity.FormBuilderName = "v-bootstrap";
            dynamicEntity.SetDataContextType(newEntityDtoStack);
            dynamicEntity.SetBinding(DataContextProperty, NewEntityDto);

            var saveButton = CreateIconButton("save", "Save", SaveNewCommand);
            saveButton.SetValue(Validation.EnabledProperty, true);
            saveButton.SetBinding(Validation.TargetProperty, NewEntityDto);

            var newForm = new HtmlGenericControl("div");
            newForm.SetBinding(VisibleProperty, NewEntityFormVisible);
            newForm.Children.Add(dynamicEntity);
            newForm.Children.Add(CreateIconButton("remove", "Cancel", CancelNew));
            newForm.Children.Add(new Literal(" "));
            newForm.Children.Add(saveButton);


            footerDiv.Children.Add(row);
            footerDiv.Children.Add(newForm);
        }

        private static HtmlGenericControl CreateDivWithClass(string classValue, params DotvvmControl[] children)
        {
            var div = new HtmlGenericControl("div");
            div.Attributes["class"] = classValue;

            foreach (var c in children)
            {
                div.Children.Add(c);
            }

            return div;
        }

        protected virtual Button CreateIconButton(string iconName, string buttonText, ICommandBinding command)
        {
            return ControlCreationHelper.IconButton(iconName, buttonText, command);
        }
    }
}
