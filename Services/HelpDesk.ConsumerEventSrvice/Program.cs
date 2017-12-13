using System.IO;
using System.Text;
using log4net.Config;
using MassTransit.Log4NetIntegration.Logging;
using Topshelf.Logging;
using System;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using Unity;
using HelpDesk.ConsumerEventService.DTO;
using Topshelf;
using System.Configuration;

namespace HelpDesk.ConsumerEventService
{
    public class Program
    {
        public static string BaseCabinetUrl { get; set; }
        public static string BaseWorkerUrl { get; set; }
        static void Main(string[] args)
        {
            BaseCabinetUrl = ConfigurationManager.AppSettings["BaseCabinetUrl"];
            BaseWorkerUrl = ConfigurationManager.AppSettings["BaseWorkerUrl"];

            ConfigureLogger();
            
            // Topshelf to use Log4Net
            Log4NetLogWriterFactory.Use();

            // MassTransit to use Log4Net
            Log4NetLogger.Use();

            HostFactory.Run(x => x.Service<RequestService>());
            /*string h = UnityConfig.GetConfiguredContainer().Resolve<IEmailTemplateService>()
                .GetEmailBody(new UserRequestAppEventSubscribeDTO {RequestId = 20456}, "RequestAppEvent");
            Console.WriteLine(h);*/

            Console.ReadKey();

        }

        static void ConfigureLogger()
        {
            const string logConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<log4net>
  <root>
    <level value=""INFO"" />
    <appender-ref ref=""console"" />
  </root>
  <appender name=""console"" type=""log4net.Appender.ColoredConsoleAppender"">
    <layout type=""log4net.Layout.PatternLayout"">
      <conversionPattern value=""%m%n"" />
    </layout>
  </appender>
</log4net>";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(logConfig)))
            {
                XmlConfigurator.Configure(stream);
            }
        }
        
    }
}