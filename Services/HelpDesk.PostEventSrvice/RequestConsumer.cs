using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Common.EventBus.AppEvents;

namespace HelpDesk.PostEventSrvice
{
    public class RequestConsumer: IConsumer<IRequestEvent>
    {
        readonly ILog log = Logger.Get<RequestConsumer>();

        public async Task Consume(ConsumeContext<IRequestEvent> context)
        {
            log.InfoFormat("RequestEventId = {0}", context.Message.RequestEventId);

            context.Respond(new RequestAppEvent
            {
                RequestEventId = context.Message.RequestEventId
            });
        }        
    }
}