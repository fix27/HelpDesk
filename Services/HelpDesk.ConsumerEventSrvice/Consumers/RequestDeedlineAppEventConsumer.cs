using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using System.Linq;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using HelpDesk.ConsumerEventService.Handlers;

namespace HelpDesk.ConsumerEventService.Consumers
{
    /// <summary>
    /// Получатель события "Истекает срок по заявке"
    /// </summary>
    public class RequestDeedlineAppEventConsumer : IConsumer<IRequestDeedlineAppEvent>
    {
        
        private readonly IAppEventHandler<IRequestDeedlineAppEvent> handler;
        private readonly ILog log;
        public RequestDeedlineAppEventConsumer(IAppEventHandler<IRequestDeedlineAppEvent> handler, ILog log)
        {
            this.handler = handler;
            this.log = log;
        }

        public async Task Consume(ConsumeContext<IRequestDeedlineAppEvent> context)
        {
            log.InfoFormat("RequestDeedlineAppEventConsumer: RequestIds.Count = {0}", context.Message.RequestIds.Count());

            await handler.Handle(context.Message);
        }
    }
}