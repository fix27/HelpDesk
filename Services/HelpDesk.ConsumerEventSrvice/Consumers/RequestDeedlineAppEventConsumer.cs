using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents;
using HelpDesk.Common.EventBus.AppEvents.Interface;

namespace HelpDesk.ConsumerEventSrvice.Consumers
{
    public class RequestDeedlineAppEventConsumer : IConsumer<IRequestDeedlineAppEvent>
    {
        readonly ILog log = Logger.Get<RequestDeedlineAppEventConsumer>();

        public async Task Consume(ConsumeContext<IRequestDeedlineAppEvent> context)
        {
            log.InfoFormat("RequestId = {0}", context.Message.RequestId);

            context.Respond(new RequestDeedlineAppEvent
            {
                RequestId = context.Message.RequestId
            });
        }        
    }
}