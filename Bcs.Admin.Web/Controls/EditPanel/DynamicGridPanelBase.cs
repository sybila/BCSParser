using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Binding;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Controls.DynamicData;
using BcsResolver.Extensions;
using Bcs.Admin.Web.ViewModels.Grids;
using DotVVM.Framework.Binding.Expressions;
using Riganti.Utils.Infrastructure.Core;

namespace Bcs.Admin.Web.Controls
{
    public abstract class DynamicGridPanelBase : PanelControl
    {
        [MarkupOptions(AllowHardCodedValue = false, Required = true)]
        public IBinding DataSourceBinding
        {
            get { return GetValue<IBinding>(DataSourceBindingProperty); }
            set { SetValue(DataSourceBindingProperty, value); }
        }

        public static readonly DotvvmProperty DataSourceBindingProperty =
            DotvvmProperty.Register<IBinding, DynamicGridPanelBase>(t => t.DataSourceBinding, null);

        protected override void CreateContent(IDotvvmRequestContext context, HtmlGenericControl bodyDiv, HtmlGenericControl footerDiv)
        {
            var toolColumn = new GridViewTemplateColumn();
            toolColumn.HeaderText = "";
            SetUpToolColumn(toolColumn);

            var grid = new GridView();
            grid.Attributes["class"] = "table table-bordered table-hover";
            grid.InlineEditing = true;
            grid.SetBinding(ItemsControl.DataSourceProperty, DataSourceBinding);
            grid.Columns.Add(toolColumn);
            grid.EmptyDataTemplate = new DelegateTemplate((f, sp, c) => c.Children.Add(new Literal("No records")));

            var dynamicDecorator = new DynamicDataGridViewDecorator();
            dynamicDecorator.ColumnPlacement = ColumnPlacement.Left;
            dynamicDecorator.Children.Add(grid);

            bodyDiv.Children.Add(dynamicDecorator);

            SetUpFooter(context, footerDiv);
        }

        protected virtual void SetUpFooter(IDotvvmRequestContext context, HtmlGenericControl footerDiv)
        {
            FooterTemplate?.BuildContent(context, footerDiv);
        }

        protected abstract void SetUpToolColumn(GridViewTemplateColumn toolColumn);
    }
}
