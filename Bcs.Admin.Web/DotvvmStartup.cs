using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;
using DotVVM.Framework.Controls.DynamicData;
using DotVVM.Framework.Controls.DynamicData.Builders;

namespace Bcs.Analyzer.DemoWeb
{
    public class DotvvmStartup : IDotvvmStartup
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("Default", "", "Views/default.dothtml");
            config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));    
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            config.Markup.AddCodeControls("cc", "Bcs.Admin.Web.Controls", "Bcs.Admin.Web");
            config.Markup.AddCodeControls("dde", "Bcs.Admin.Web.Controls.Dynamic", "Bcs.Admin.Web");
            config.Markup.AddCodeControls("dde", "Bcs.Admin.Web.Controls.EditPanel", "Bcs.Admin.Web");
            config.Markup.AddMarkupControl("cc", "ModalAlert", "Controls/ModalAlert.dotcontrol");
            config.Markup.AddMarkupControl("cc", "StatusAlerts", "Controls/statusAlerts.dotcontrol");
            config.AddDynamicDataConfiguration();
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.Register("bootstrap-css", new StylesheetResource
            {
                Location = new UrlResourceLocation("~/lib/bootstrap/dist/css/bootstrap.min.css")
            });
            config.Resources.Register("bootstrap-theme", new StylesheetResource
            {
                Location = new UrlResourceLocation("~/lib/bootstrap/dist/css/bootstrap-theme.min.css"),
                Dependencies = new[] { "bootstrap-css" }
            });
            config.Resources.Register("bootstrap", new ScriptResource
            {
                Location = new UrlResourceLocation("~/lib/bootstrap/dist/js/bootstrap.min.js"),
                Dependencies = new[] { "bootstrap-css" , "jquery" }
            });
            config.Resources.Register("jquery", new ScriptResource
            {
                Location = new UrlResourceLocation("~/lib/jquery/dist/jquery.min.js")
            });
            config.Resources.Register("site", new StylesheetResource
            {
                Location = new UrlResourceLocation("~/site.css"),
                Dependencies = new[] { "bootstrap"}
            });
            config.Resources.Register("code-editor", new ScriptResource
            {
                Location = new UrlResourceLocation("~/CodeEditor.js"),
            });
        }
    }
}
