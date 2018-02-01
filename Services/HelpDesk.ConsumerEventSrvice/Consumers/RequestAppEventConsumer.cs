using System.Threading.Tasks;
using MassTransit;
using MassTransit.Logging;
using HelpDesk.EventBus.Common.AppEvents.Interface;
using System.Collections.Generic;
using HelpDesk.ConsumerEventService.Sender;
using HelpDesk.ConsumerEventService.Handlers;

namespace HelpDesk.ConsumerEventService.Consumers
{
    /// <summary>
    /// Получатель события "Изменилось состояние заявки"
    /// </summary>
    public class RequestAppEventConsumer : IConsumer<IRequestAppEvent>
    {
        
        private readonly IAppEventHandler<IRequestAppEvent> handler;
        private readonly ILog log;
        
        public RequestAppEventConsumer(IAppEventHandler<IRequestAppEvent> handler, ILog log, IEnumerable<ISender> senders)
        {
            this.handler = handler;
            this.log = log;
        }

        public async Task Consume(ConsumeContext<IRequestAppEvent> context)
        {
            log.InfoFormat("RequestAppEventConsumer: RequestEventId = {0}", context.Message.RequestEventId);

            await handler.Handle(context.Message);
        }        
    }
}