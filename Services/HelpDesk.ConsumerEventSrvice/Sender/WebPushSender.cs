using HelpDesk.ConsumerEventService.DTO;
using MassTransit.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
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
            var model = new
            {
                app_id = appId,
                contents = new { ru = subject },
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
