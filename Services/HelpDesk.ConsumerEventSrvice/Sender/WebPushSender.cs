using HelpDesk.ConsumerEventService.DTO;
using MassTransit.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System;
using HelpDesk.ConsumerEventService.Resources;

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

        public Task SendAsync(BaseUserEventSubscribeDTO msg)
        {
            string body = null;
            string title = null;

            var msg1 = msg as UserRequestAppEventSubscribeDTO;
            if (msg1 != null)
            {
                title = msg1.Request.Id.ToString();
                body = String.Format(Resource.Subject_RequestAppEventConsumer, msg1.Request.Id, msg1.Request.StatusName); ;
            }

            var msg2 = msg as UserDeedlineAppEventSubscribeDTO;
            if (msg2 != null)
            {
                title = Resource.Subject_UserPasswordRecoveryAppEventConsumer;
                body = String.Join(", ", msg2.Items.Select(r => r.Id));
            }

            var model = new
            {
                app_id = appId,
                contents = new { en = body },
                headings = new { en = title },
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
