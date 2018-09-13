using HelpDesk.DataService.Interface;
using HelpDesk.Entity;
using System.Collections.Generic;
using System.IO;
using HelpDesk.CabinetWebApp.App.Resources;

namespace HelpDesk.CabinetWebApp.Helpers
{
    public class JsResourceHelper
    {
        private readonly ISettingsService settingsService;

        public JsResourceHelper(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }
        public void Set()
        {
            Settings settings = settingsService.Get();
            JsResource t4JsResource = new JsResource();
            t4JsResource.Session = new Dictionary<string, object>();
            t4JsResource.Session.Add("settings", settings);
            t4JsResource.Initialize();

            string pageContent = t4JsResource.TransformText();
            File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/App/Resources/jsResource.js"), pageContent);
        }
    }
}