using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Configuration;
using DotVVM.Framework.Controls.DynamicData.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls.Dynamic
{
    public class ConventionComboBoxGridColumnProvider : ComboBoxGridColumnProvider
    {
        private readonly ComboBoxConventions comboBoxConventions;

        public ConventionComboBoxGridColumnProvider(ComboBoxConventions comboBoxConventions)
        {
            this.comboBoxConventions = comboBoxConventions;
        }

        public override bool CanHandleProperty(PropertyInfo propertyInfo, DynamicDataContext context)
        {
            var matchedConvention = comboBoxConventions.Conventions.FirstOrDefault(c => c.Match.IsMatch(propertyInfo));
            if (matchedConvention == null)
            {
                return false;
            }

            // store the convention in the context
            context.StateBag[new StateBagKey(this, propertyInfo)] = matchedConvention;
            return true;
        }

        protected override string GetDisplayMember(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return base.GetDisplayMember(property, context) ?? GetConvention(property, context).Settings.DisplayMember;
        }

        protected override string GetValueMember(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return base.GetValueMember(property, context) ?? GetConvention(property, context).Settings.ValueMember;
        }

        protected override string GetEmptyItemText(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return base.GetEmptyItemText(property, context) ?? GetConvention(property, context).Settings.EmptyItemText;
        }

        protected override string GetDataSourceBindingExpression(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return base.GetDataSourceBindingExpression(property, context) ?? GetConvention(property, context).Settings.DataSourceBinding;
        }

        private ComboBoxConvention GetConvention(PropertyDisplayMetadata property, DynamicDataContext context)
        {
            return (ComboBoxConvention)context.StateBag[new StateBagKey(this, property.PropertyInfo)];
        }
    }
}
