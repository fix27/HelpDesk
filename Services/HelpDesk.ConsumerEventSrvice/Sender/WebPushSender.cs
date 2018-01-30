using HelpDesk.ConsumerEventService.DTO;
using HelpDesk.ConsumerEventService.EmailTemplateServices;
using HelpDesk.ConsumerEventService.Resources;
using MassTransit.Logging;
using System;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HelpDesk.ConsumerEventService.Sender
{
    /// <summary>
    /// Отправщик Push-сообщений в https://onesignal.com
    /// </summary>
    public class WebPushSender : ISender
    {
        private readonly string appId;
        private readonly ILog log;
        private readonly HttpClient client = new HttpClient();
        public WebPushSender(string appId, ILog log)
        {
            this.appId = appId;
            this.log = log;
        }

        public Task SendAsync(UserEventSubscribeDTO msg, string subject, string messageTemplateName)
        {
            return client.PostAsync("https://onesignal.com/api/v1/notifications", null);
        }        
        
    }
}
