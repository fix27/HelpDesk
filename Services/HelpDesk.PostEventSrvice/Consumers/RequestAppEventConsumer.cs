using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.PostEventSrvice.Consumers
{
    public class RequestAppEventConsumer : IConsumer<IRequestAppEvent>
    {
        readonly ILog log = Logger.Get<RequestAppEventConsumer>();

        public async Task Consume(ConsumeContext<IRequestAppEvent> context)
        {
            log.InfoFormat("RequestEventId = {0}", context.Message.RequestEventId);

            context.Respond(new RequestAppEvent
            {
                RequestEventId = context.Message.RequestEventId
            });
        }        
    }
}