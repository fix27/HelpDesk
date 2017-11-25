using HelpDesk.ConsumerEventSrvice.DTO;
using RazorEngine.Templating;
using System;
using System.Configuration;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.WebPages.Razor.Configuration;

namespace HelpDesk.ConsumerEventSrvice.Sender
{
    public class EmailSender: ISender
    {
        private readonly string templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
        public void Send(UserEventSubscribeDTO evnt, string subject, string messageTemplate = null)
        {
            /*
             * Quick benchmark demonstrating the effect of using RazorEngine's caching functionality.
             * 
             * We'll generate the same email 3 times in a row - first without caching and then with caching.
             * We'll measure and print how long each try takes.
             * 
             * Also demonstrates how to retrieve the list of default namespaces to use when generating the template class
             * from the ASP.NET MVC configuration section in the web.config file.
             */

            var welcomeEmailTemplate = File.ReadAllText(Path.Combine(templateFolderPath, !string.IsNullOrWhiteSpace(messageTemplate)? $"{messageTemplate}.cshtml": "Default.cshtml"));
            
            var templateService = new TemplateService();
            
            addDefaultNamespacesFromWebConfig(templateService);


            var emailHtmlBody = templateService.Parse(welcomeEmailTemplate, evnt, null, messageTemplate);
            sendEmail(evnt.Email, subject, emailHtmlBody);
        }

        /// <summary>
        /// Add the namespaces found in the ASP.NET MVC configuration section of the Web.config file to the provided TemplateService instance.
        /// </summary>
        private void addDefaultNamespacesFromWebConfig(TemplateService templateService)
        {
            var webConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web.config");
            if (!File.Exists(webConfigPath))
                return;

            var fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = webConfigPath };
            var configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var razorConfig = configuration.GetSection("system.web.webPages.razor/pages") as RazorPagesSection;

            if (razorConfig == null)
                return;

            foreach (NamespaceInfo namespaceInfo in razorConfig.Namespaces)
            {
                templateService.AddNamespace(namespaceInfo.Namespace);
            }
        }

        private void sendEmail(string email, string subject, string body)
        {
            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            SmtpClient smtpClient = new SmtpClient();

            try
            {
                smtpClient.Send(new MailMessage(smtpSection.From, email, subject, body));
            }
            catch (Exception ex)
            {
                //log.Error("Application error", ex);
            }

        }
    }
}
