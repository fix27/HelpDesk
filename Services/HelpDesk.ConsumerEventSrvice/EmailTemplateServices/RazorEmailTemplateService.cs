using HelpDesk.ConsumerEventService.DTO;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.IO;

namespace HelpDesk.ConsumerEventService.EmailTemplateServices
{
    /// <summary>
    /// RazorEngine Email-шаблонизатор. Шаблоны в виде отдельных cshtml-файлов
    /// </summary>
    public class RazorEmailTemplateService : IEmailTemplateService
    {
        private readonly string templateFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
        public string GetEmailBody(UserEventSubscribeDTO evnt, string messageTemplate)
        {
            if (evnt == null)
                throw new ArgumentException("messageTemplate is evnt");

            if (String.IsNullOrEmpty(messageTemplate))
                throw new ArgumentException("messageTemplate is null");

            var config = new TemplateServiceConfiguration();
            config.DisableTempFileLocking = true;
            config.CachingProvider = new DefaultCachingProvider(t => { });
            config.TemplateManager = new DelegateTemplateManager(name =>
            {
                string path = Path.Combine(templateFolderPath, name);
                return File.ReadAllText(path);
            });


            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
            
            var template = File.ReadAllText(Path.Combine(templateFolderPath, $"{messageTemplate}.cshtml"));
            
            
            return Engine.Razor.RunCompile(template, messageTemplate, evnt.GetType(), evnt);
        }
        
    }
}
