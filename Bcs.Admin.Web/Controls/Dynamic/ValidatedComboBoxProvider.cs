using DotVVM.Framework.Controls.DynamicData.Configuration;
using DotVVM.Framework.Controls.DynamicData.PropertyHandlers.FormEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls.Dynamic
{
    public class ValidatedComboBoxProvider : ComboBoxConventionFormEditorProvider
    {
        public override bool CanValidate => true;

        public ValidatedComboBoxProvider(ComboBoxConventions comboBoxConventions) : base(comboBoxConventions)
        {
        }
    }
}
