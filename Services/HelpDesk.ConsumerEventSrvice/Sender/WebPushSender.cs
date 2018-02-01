using HelpDesk.ConsumerEventService.DTO;
using MassTransit.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System;

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
            string msgBody = null;
            string msgTitle = null;

            //TODO: ... вроде этот switch(Type) пока не расползается за пределы метода
            if (msg is UserRequestAppEventSubscribeDTO)
            {
                RequestDTO request = ((UserRequestAppEventSubscribeDTO)msg).Request;
                msgTitle = request.Id.ToString();
                msgBody = subject;
            }

            if (msg is UserDeedlineAppEventSubscribeDTO)
            {
                IEnumerable<RequestDTO> requests = ((UserDeedlineAppEventSubscribeDTO)msg).Items;
                msgTitle = subject;
                msgBody = String.Join(", ", requests.Select(r => r.Id));
            }


            var model = new
            {
                app_id = appId,
                contents = new { en = msgBody },
                headings = new { en = msgTitle },
                filters = new object[] 
                {
                    new { field = "tag" },
                    new { key = "userEmail" },
                    new { relation = "=" },
                    new { value = msg.Email }
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            return client.PostAsync("https://onesignal.com/api/v1/notifications", content);
        }        
        
    }
}
