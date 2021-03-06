﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bcs.Admin.Web.Controls.EditPanel;
using Bcs.Admin.Web.Utils;
using Bcs.Admin.Web.ViewModels;
using Bcs.Admin.Web.ViewModels.Grids;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Compilation.ControlTree;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Metadata;
using DotVVM.Framework.Controls.DynamicData.PropertyHandlers.FormEditors;

namespace Bcs.Admin.Web.Controls.Dynamic
{
    public class PermanentViewBuilder {

        private StringBuilder builder = new StringBuilder();

        public void Control(DotvvmControl c)
        {
            if (c is HtmlGenericControl htmlGeneric)
            {
                PrintGeneric(htmlGeneric);
            }
            else {
                DotvvmControl(c);
            }
        }

        public void DotvvmControl(DotvvmControl c)
        {
            builder.Append($"<dot:{c.GetType().Name}>");

            foreach (var child in c.Children)
            {
                Control(child);
            }
            builder.Append($"</dot:{c.GetType().Name}>");
        }

        public void PrintGeneric(HtmlGenericControl htmlGenericControl)
        {
            builder.Append($"<{htmlGenericControl.TagName}>");

            foreach (var attribute in htmlGenericControl.Attributes)
            {
                builder.Append(attribute.Value);             
            }

            foreach (var child in htmlGenericControl.Children)
            {
                Control(child);
            }

            builder.Append($"<{htmlGenericControl.TagName}>");
        }

        public string GetView() => builder.ToString();
    }

    public class EditableDynamicGridEditorProvider : FormEditorProviderBase
    {
        public override bool RenderDefaultLabel => false;

        public override bool CanHandleProperty(PropertyInfo propertyInfo, DynamicDataContext context)
        {
            return propertyInfo.PropertyType.IsAssignableToGenericType(typeof(IEditableGrid<,>));
        }

        public override void CreateControl(DotvvmControl container, PropertyDisplayMetadata property, DynamicDataContext context)
        {
            var gridDataContext = property.PropertyInfo.PropertyType;

            var grid = new EditableDynamicGridPanel(context.BindingCompilationService);
            container.Children.Add(grid);
            grid.SetDataContextType(DataContextStack.Create(gridDataContext, container.GetDataContextType()));

            var cssClass = ControlHelpers.ConcatCssClasses(ControlCssClass, property.Styles?.FormControlCssClass);
            if (!string.IsNullOrEmpty(cssClass))
            {
                grid.Attributes["class"] = cssClass;
            }

            grid.SetBinding(DotvvmBindableObject.DataContextProperty, context.CreateValueBinding(property.PropertyInfo.Name));
            grid.SetBinding(HtmlGenericControl.VisibleProperty, context.CreateValueBinding($"ParentEntityId > 0", gridDataContext));

            grid.HeadingText = property.DisplayName;
            grid.DataSourceBinding = context.CreateValueBinding("DataSet", gridDataContext);
            grid.NewEntityFormVisible = context.CreateValueBinding("NewRow != null", gridDataContext);
            grid.NewEntityDto = context.CreateValueBinding("NewRow", gridDataContext);

            grid.EditCommand = context.CreateCommandBinding("_parent.Edit(_this)", gridDataContext, GetItemViewModelType(grid.DataSourceBinding));
            grid.DeleteCommand = context.CreateCommandBinding("_parent.DeleteAsync(_this)", gridDataContext, GetItemViewModelType(grid.DataSourceBinding));
            grid.CancelCommand = context.CreateCommandBinding("_parent.Cancel()", gridDataContext, GetItemViewModelType(grid.DataSourceBinding));
            grid.SaveCommand = context.CreateCommandBinding("_parent.SaveEditAsync(_this)", gridDataContext, GetItemViewModelType(grid.DataSourceBinding));
            grid.SaveNewCommand = context.CreateCommandBinding("SaveNewAsync()", gridDataContext);
            grid.CancelNew = context.CreateCommandBinding("Cancel()", gridDataContext);
            grid.AddCommand = context.CreateCommandBinding("Add()", gridDataContext);

            if (property.PropertyInfo.PropertyType.IsAssignableToGenericType(typeof(IEditableLinkGrid<,>)))
            {
                grid.LinkEntity = context.CreateCommandBinding("LinkAsync()", gridDataContext);
                grid.LinkEntitySearchSelect = context.CreateValueBinding("EntitySearchSelect", gridDataContext);
            }
            if (property.PropertyInfo.PropertyType.IsAssignableTo(typeof(ICollapsible)))
            {
                grid.CollapsedBinding = context.CreateValueBinding($"{nameof(ICollapsible.IsCollapsed)}", gridDataContext);
            }
            if (property.PropertyInfo.PropertyType.IsAssignableTo(typeof(IStatusReporter)))
            {
                grid.SuccessMessage = context.CreateValueBinding("SuccessMessage", gridDataContext);
                grid.Errors = context.CreateValueBinding("Errors", gridDataContext);
            }

            //var delf = new PermanentViewBuilder();
            //delf.Control(grid);
            //var a = delf.GetView();
        }

        //TODO: Merge when integrated with dynamic data
        protected virtual Type GetItemViewModelType(IValueBinding dataSourceBinding)
        {
            var dataSourceType = dataSourceBinding.ResultType;

            if (dataSourceType.GetTypeInfo().IsGenericType && dataSourceType.GetGenericTypeDefinition() == typeof(GridViewDataSet<>))
            {
                return dataSourceType.GetTypeInfo().GetGenericArguments()[0];
            }

            var enumerableType = dataSourceType.GetTypeInfo().GetInterfaces().FirstOrDefault(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (enumerableType != null)
            {
                return enumerableType.GetGenericArguments()[0];
            }

            return typeof(object);
        }
    }
}
