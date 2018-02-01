using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.Data.Query;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.DTO;
using System.Collections.Generic;
using System.Linq;
using HelpDesk.ConsumerEventService.Query;
using HelpDesk.ConsumerEventService.Resources;
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

        static object lockObj = new object();
        public async Task Consume(ConsumeContext<IRequestDeedlineAppEvent> context)
        {
            log.InfoFormat("RequestDeedlineAppEventConsumer: RequestIds.Count = {0}", context.Message.RequestIds.Count());

            await handler.Handle(context.Message);
        }
    }
}