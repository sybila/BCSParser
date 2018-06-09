using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Metadata;
using DotVVM.Framework.Controls.DynamicData.PropertyHandlers.FormEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls.Dynamic
{
    public class CodeEditorAttribute : Attribute
    {
        public CodeEditorAttribute(string keyUpMethodName = null)
        {
            KeyUpMethodName = keyUpMethodName;
        }

        public string KeyUpMethodName { get; }
    }

    public class CodeEditorProvider : FormEditorProviderBase
    {
        public override bool CanHandleProperty(PropertyInfo propertyInfo, DynamicDataContext context)
        {
            return GetCodeEditorAttribute(propertyInfo) != null && propertyInfo.PropertyType == typeof(string);
        }

        public override void CreateControl(DotvvmControl container, PropertyDisplayMetadata property, DynamicDataContext context)
        {
            var codeControl = new CodeEditor();
            container.Children.Add(codeControl);

            var cssClass = ControlHelpers.ConcatCssClasses(ControlCssClass, property.Styles?.FormControlCssClass);

            codeControl.Attributes["class"] = "form-control code-editor";

            var attribute = GetCodeEditorAttribute(property.PropertyInfo);

            codeControl.SetBinding(CodeEditor.HtmlProperty, context.CreateValueBinding(property.PropertyInfo.Name));

            if (attribute.KeyUpMethodName != null)
            {
                codeControl.SetBinding(CodeEditor.KeyDownProperty, context.CreateCommandBinding($"{attribute.KeyUpMethodName}()"));
            }
        }

        private static CodeEditorAttribute GetCodeEditorAttribute(PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttribute<CodeEditorAttribute>();
        }
    }
}
