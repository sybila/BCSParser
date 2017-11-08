using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using DotVVM.Framework.Controls.DynamicData.Metadata;
using DotVVM.Framework.Controls.DynamicData.PropertyHandlers.GridColumns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls.Dynamic
{
    public class ComboBoxGridColumnProvider : GridColumnProviderBase
    {
        public override bool CanHandleProperty(PropertyInfo propertyInfo, DynamicDataContext context)
        {
            return GetSettings(propertyInfo) != null;
        }

        protected override GridViewColumn CreateColumnCore(GridView gridView, PropertyDisplayMetadata property, DynamicDataContext context)
        {
            var comboBox = new GridViewComboBoxColumn()
            {
                DisplayMember = GetDisplayMember(property, context),
                ValueMember = GetValueMember(property, context),
                EmptyItemText = GetEmptyItemText(property, context)
            };

            comboBox.SetBinding(GridViewComboBoxColumn.SelectedValueBindingProperty, context.CreateValueBinding(property.PropertyInfo.Name));
            comboBox.SetBinding(GridViewComboBoxColumn.DataSourceBindingProperty, GetDataSourceBinding(property, context, comboBox));

            return comboBox;
        }

        private ComboBoxSettingsAttribute GetSettings(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<ComboBoxSettingsAttribute>();
        }

        /// <summary>
        /// Compiles the DataSource binding expression to a value binding.
        /// </summary>
        protected virtual ValueBindingExpression GetDataSourceBinding(PropertyDisplayMetadata property, DynamicDataContext context, GridViewComboBoxColumn comboBox)
        {
            var dataSourceBindingExpression = GetDataSourceBindingExpression(property, context);
            if (string.IsNullOrEmpty(dataSourceBindingExpression))
            {
                throw new Exception($"The DataSource binding expression for property {property.PropertyInfo} must be specified!");
            }

            return context.CreateValueBinding(dataSourceBindingExpression);
        }

        /// <summary>
        /// Gets the DataSource binding expression for the ComboBox control from the ComboBoxSettingsAttribute.
        /// </summary>
        protected virtual string GetDataSourceBindingExpression(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return GetSettings(property.PropertyInfo)?.DataSourceBinding;
        }

        /// <summary>
        /// Gets the EmptyItemText for the ComboBox control from the ComboBoxSettingsAttribute.
        /// </summary>
        protected virtual string GetEmptyItemText(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return GetSettings(property.PropertyInfo)?.EmptyItemText;
        }

        /// <summary>
        /// Gets the ValueMember for the ComboBox control from the ComboBoxSettingsAttribute.
        /// </summary>
        protected virtual string GetValueMember(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return GetSettings(property.PropertyInfo)?.ValueMember;
        }

        /// <summary>
        /// Gets the DisplayMember for the ComboBox control from the ComboBoxSettingsAttribute.
        /// </summary>
        protected virtual string GetDisplayMember(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return GetSettings(property.PropertyInfo)?.DisplayMember;
        }
    }
}
