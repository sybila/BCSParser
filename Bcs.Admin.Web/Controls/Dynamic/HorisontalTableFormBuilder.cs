using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Builders;
using DotVVM.Framework.Controls.DynamicData.Metadata;

namespace Bcs.Admin.Web.Controls.Dynamic
{
    public class HorisontalTableFormBuilder : TableDynamicFormBuilder
    {
        public override void BuildForm(DotvvmControl hostControl, DynamicDataContext dynamicDataContext)
        {
            var entityPropertyListProvider = dynamicDataContext.RequestContext.Configuration.ServiceLocator.GetService<IEntityPropertyListProvider>();

            var table = InitializeTable(hostControl, dynamicDataContext);

            var properties = GetPropertiesToDisplay(dynamicDataContext, entityPropertyListProvider);

            var labelRow = new HtmlGenericControl("tr");
            table.Children.Add(labelRow);

            var editorRow = new HtmlGenericControl("tr");
            table.Children.Add(editorRow);

            foreach (var property in properties)
            {
                var editorProvider = FindEditorProvider(property, dynamicDataContext);
                if (editorProvider == null) continue;

                var headerCell = new HtmlGenericControl("th");
                labelRow.Children.Add(headerCell);

                var editorCell = new HtmlGenericControl("td");
                editorRow.Children.Add(editorCell);

                InitializeControlLabel(labelRow, headerCell, editorProvider, property, dynamicDataContext);
                InitializeControlEditor(editorRow, editorCell, editorProvider, property, dynamicDataContext);
                InitializeValidation(editorRow, labelRow, editorRow, editorProvider, property, dynamicDataContext);
            }
        }

        protected override HtmlGenericControl InitializeTable(DotvvmControl hostControl, DynamicDataContext dynamicDataContext)
        {
            var table = new HtmlGenericControl("table");
            table.Attributes["class"] = "table table-bordered";

            hostControl.Children.Add(table);
            return table;
        }
    }
}
