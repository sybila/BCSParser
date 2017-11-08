using Bcs.Admin.Web.Controls.Dynamic;
using Bcs.Admin.Web.ViewModels;
using BcsAdmin.BL.Dto;
using DotVVM.Framework.Controls.DynamicData.Annotations;
using DotVVM.Framework.Controls.DynamicData.Configuration;
using DotVVM.Framework.Controls.DynamicData.PropertyHandlers.FormEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bcs.Admin.Web
{
    public class AppDynamicDataConfiguration : DynamicDataConfiguration
    {
        public AppDynamicDataConfiguration() : base()
        {
            SetComboBoxConventions();
        }

        private void SetComboBoxConventions()
        {
            var conventions = new ComboBoxConventions();
            conventions.Register(p => p.Name.Equals(nameof(BiochemicalEntityDetailDto.SelectedHierarchyType)), new ComboBoxSettingsAttribute()
            {
                DataSourceBinding = nameof(BiochemicalEntityDetailDto.HierarchyTypes),
                DisplayMember = nameof(BiochemicalEntityTypeDto.Name),
                ValueMember = nameof(BiochemicalEntityTypeDto.Id),
                EmptyItemText = "(select entity type)"
            });
            conventions.Register(p => p.Name.Equals(nameof(BiochemicalEntityLinkDto.HierarchyType)), new ComboBoxSettingsAttribute()
            {
                DataSourceBinding = $"_parent.{nameof(BiochemicalEntityDetailDto.HierarchyTypes)}",
                DisplayMember = nameof(BiochemicalEntityTypeDto.Name),
                ValueMember = nameof(BiochemicalEntityTypeDto.Id),
                EmptyItemText = "(select entity type)"
            });

            var provider = new ComboBoxConventionFormEditorProvider(conventions);
            var columnProvider = new ConventionComboBoxGridColumnProvider(conventions);
            FormEditorProviders.Insert(0, provider);
            GridColumnProviders.Insert(0, columnProvider);
        }
    }
}
