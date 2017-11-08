using BcsResolver.Extensions;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls
{
    [ControlMarkupOptions(AllowContent = false)]
    public class GridViewComboBoxColumn : GridViewColumn
    {
        public string EmptyItemText
        {
            get { return (string)GetValue(EmptyItemTextProperty); }
            set { SetValue(EmptyItemTextProperty, value); }
        }
        public static readonly DotvvmProperty EmptyItemTextProperty
            = DotvvmProperty.Register<string, GridViewComboBoxColumn>(c => c.EmptyItemText, string.Empty);

        /// <summary>
        /// Gets or sets the name of property in the DataSource collection that will be displayed in the control.
        /// </summary>
        public string DisplayMember
        {
            get { return (string)GetValue(DisplayMemberProperty); }
            set { SetValue(DisplayMemberProperty, value); }
        }
        public static readonly DotvvmProperty DisplayMemberProperty =
            DotvvmProperty.Register<string, GridViewComboBoxColumn>(t => t.DisplayMember, "");

        /// <summary>
        /// Gets or sets the name of property in the DataSource collection that will be passed to the SelectedValue property when the item is selected.
        /// </summary>
        public string ValueMember
        {
            get { return (string)GetValue(ValueMemberProperty); }
            set { SetValue(ValueMemberProperty, value); }
        }
        public static readonly DotvvmProperty ValueMemberProperty =
            DotvvmProperty.Register<string, GridViewComboBoxColumn>(t => t.ValueMember, "");

        /// <summary>
        /// Gets or sets the source collection or a GridViewDataSet that contains data in the control.
        /// </summary>
        [MarkupOptions(AllowHardCodedValue = false)]
        public IBinding DataSourceBinding
        {
            get { return GetValue<IBinding>(DataSourceBindingProperty); }
            set { SetValue(DataSourceBindingProperty, value); }
        }

        public static readonly DotvvmProperty DataSourceBindingProperty =
            DotvvmProperty.Register<object, GridViewComboBoxColumn>(t => t.DataSourceBinding, null);

        [MarkupOptions(AllowHardCodedValue = false, Required = true)]
        public object SelectedValueBinding
        {
            get { return GetValue<IBinding>(SelectedValueBindingProperty); }
            set { SetValue(SelectedValueBindingProperty, value); }
        }

        public static readonly DotvvmProperty SelectedValueBindingProperty =
            DotvvmProperty.Register<object, GridViewComboBoxColumn>(t => t.SelectedValueBinding);

        protected override string GetSortExpression()
        {
            if (string.IsNullOrEmpty(SortExpression))
            {
                return "" ??
                    throw new DotvvmControlException(this, $"The 'ValueBinding' property must be set on the '{GetType()}' control!");
            }
            else
            {
                return SortExpression;
            }
        }

        public override void CreateControls(IDotvvmRequestContext context, DotvvmControl container)
        {
            container.Children.Add(CreateComboBox(false));
        }

        public override void CreateEditControls(IDotvvmRequestContext context, DotvvmControl container)
        {
            container.Children.Add(CreateComboBox(true));
        }

        private ComboBox CreateComboBox(bool enabled)
        {
            var comboBox = new ComboBox();
            comboBox.SetBinding(Selector.SelectedValueProperty, GetValueBinding(SelectedValueBindingProperty));
            
            comboBox.DisplayMember = DisplayMember;
            comboBox.EmptyItemText = EmptyItemText;
            comboBox.ValueMember = ValueMember;
            comboBox.Enabled = enabled;
            comboBox.SetBinding(ItemsControl.DataSourceProperty, GetValueBinding(DataSourceBindingProperty));
            return comboBox;
        }
    }
}
