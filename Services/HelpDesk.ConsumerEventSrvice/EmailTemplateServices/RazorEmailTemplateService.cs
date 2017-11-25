using HelpDesk.ConsumerEventSrvice.DTO;
using RazorEngine.Templating;
using System;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using System.Web.WebPages.Razor.Configuration;

namespace HelpDesk.ConsumerEventSrvice.EmailTemplateServices
{
    public class RazorEmailTemplateService : IEmailTemplateService
    {
        private readonly string templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
        public string GetEmailBody(UserEventSubscribeDTO evnt, string messageTemplate = null)
        {
            var emailTemplate = File.ReadAllText(Path.Combine(templateFolderPath, !string.IsNullOrWhiteSpace(messageTemplate) ? $"{messageTemplate}.cshtml" : "Default.cshtml"));

            var templateService = new TemplateService();

            addDefaultNamespacesFromWebConfig(templateService);
            
            return templateService.Parse(emailTemplate, evnt, null, messageTemplate);
        }

        /// <summary>
        /// Add the namespaces found in the ASP.NET MVC configuration section of the Web.config file to the provided TemplateService instance.
        /// </summary>
        private void addDefaultNamespacesFromWebConfig(TemplateService templateService)
        {
            var webConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web.config");
            if (!File.Exists(webConfigPath))
                return;

            var fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = webConfigPath
            };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var razorConfig = configuration.GetSection("system.web.webPages.razor/pages") as RazorPagesSection;

            if (razorConfig == null)
                return;

            foreach (NamespaceInfo namespaceInfo in razorConfig.Namespaces)
            {
                templateService.AddNamespace(namespaceInfo.Namespace);
            }
        }
    }
}
