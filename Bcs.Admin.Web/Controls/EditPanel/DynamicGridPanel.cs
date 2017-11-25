using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Controls;

namespace Bcs.Admin.Web.Controls.EditPanel
{
    public class DynamicGridPanel : DynamicGridPanelBase
    {
        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement, Required = true)]
        [ControlPropertyBindingDataContextChange(nameof(DataSourceBinding))]
        [CollectionElementDataContextChange(1)]
        public ITemplate RowToolsTemplate
        {
            get { return (ITemplate)GetValue(RowToolsTemplateProperty); }
            set { SetValue(RowToolsTemplateProperty, value); }
        }
        public static readonly DotvvmProperty RowToolsTemplateProperty
            = DotvvmProperty.Register<ITemplate, DynamicGridPanel>(c => c.RowToolsTemplate, null);


        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
        [ControlPropertyBindingDataContextChange(nameof(DataSourceBinding))]
        [CollectionElementDataContextChange(1)]
        public ITemplate RowEditToolsTemplate
        {
            get { return (ITemplate)GetValue(RowEditToolsTemplateProperty); }
            set { SetValue(RowEditToolsTemplateProperty, value); }
        }
        public static readonly DotvvmProperty RowEditToolsTemplateProperty
            = DotvvmProperty.Register<ITemplate, DynamicGridPanel>(c => c.RowEditToolsTemplate, null);

        protected override void SetUpToolColumn(GridViewTemplateColumn toolColumn)
        {
            toolColumn.ContentTemplate = RowToolsTemplate;
            toolColumn.EditTemplate = RowEditToolsTemplate;
        }
    }
}
