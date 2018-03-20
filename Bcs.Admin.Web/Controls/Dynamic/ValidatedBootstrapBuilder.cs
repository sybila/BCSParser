using DotVVM.Framework.Controls;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Builders;
using DotVVM.Framework.Controls.DynamicData.Metadata;
using DotVVM.Framework.Controls.DynamicData.PropertyHandlers.FormEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web.Controls.Dynamic
{
    public class ValidatedBootstrapBuilder : BootstrapFormGroupBuilder
    {
        protected override void InitializeValidation(HtmlGenericControl formGroup, HtmlGenericControl labelElement, HtmlGenericControl controlElement, IFormEditorProvider editorProvider, PropertyDisplayMetadata property, DynamicDataContext dynamicDataContext)
        {
             if (dynamicDataContext.ValidationMetadataProvider.GetAttributesForProperty(property.PropertyInfo).OfType<System.ComponentModel.DataAnnotations.RequiredAttribute>().Any())
            {
                labelElement.Attributes["class"] = ControlHelpers.ConcatCssClasses(labelElement.Attributes["class"] as string, "dynamicdata-required");
            }

            if (editorProvider.CanValidate)
            {
                var validatedValueBinding = editorProvider.GetValidationValueBinding(property, dynamicDataContext);

                foreach (var child in controlElement.Children)
                {
                    child.SetValue(Validator.ValueProperty, validatedValueBinding);
                    child.SetValue(Validator.InvalidCssClassProperty, "is-invalid");
                }

                var validator = new Validator();
                validator.SetBinding(Validator.ValueProperty, validatedValueBinding);
                validator.SetValue(Validator.ShowErrorMessageTextProperty, true);
                validator.Attributes["class"] = "label label-danger";
                controlElement.Children.Add(validator);
            }
        }
    }
}
