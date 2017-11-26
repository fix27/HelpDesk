using HelpDesk.ConsumerEventService.DTO;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.IO;

namespace HelpDesk.ConsumerEventService.EmailTemplateServices
{
    public class RazorEmailTemplateService : IEmailTemplateService
    {
        private readonly string templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
        public string GetEmailBody(UserEventSubscribeDTO evnt, string messageTemplate = null)
        {
            var config = new TemplateServiceConfiguration();
            config.DisableTempFileLocking = true;
            config.CachingProvider = new DefaultCachingProvider(t => { });
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
            var template = File.ReadAllText(Path.Combine(templateFolderPath, !string.IsNullOrWhiteSpace(messageTemplate) ? $"{messageTemplate}.cshtml" : "Default.cshtml"));
            
            
            return Engine.Razor.RunCompile(template, messageTemplate, evnt.GetType(), evnt);
        }
        
    }
}
