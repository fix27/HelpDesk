using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using HelpDesk.ConsumerEventService.Resources;
using MassTransit.Logging;
using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;

namespace HelpDesk.ConsumerEventService.Sender
{
    /// <summary>
    /// Отправщик Push-сообщений в https://onesignal.com
    /// </summary>
    public class WebPushSender : ISender
    {
        private readonly string appId;
        private readonly ILog log;
        public WebPushSender(string appId, ILog log)
        {
            this.appId = appId;
            this.log = log;
        }

        public void Send(UserEventSubscribeDTO msg, string subject, string messageTemplateName)
        {
            
        }        
        
    }
}
